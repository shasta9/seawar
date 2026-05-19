package main

import (
	"encoding/json"
	"flag"
	"fmt"
	"log"
	"math/rand"
	"os"
	"time"

	"github.com/shasta9/seawar/internal/mapgen"
	"github.com/shasta9/seawar/pkg/mapdata"
)

const (
	mapWidth    = 64
	mapHeight   = 64
	maxAttempts = 10
	version     = "seawar-mapgen v0.1"
)

func main() {
	var (
		seed          = flag.Int64("seed", 0, "RNG seed (0 = random, printed to stderr for reproducibility)")
		name          = flag.String("name", "Unnamed", "Map name")
		desc          = flag.String("desc", "", "Map description")
		islandDensity = flag.Float64("island-density", 0.35, "Controls island density: probability of a cell being land in the initial noise pass before smoothing. Higher values produce more land mass. Due to cellular automata smoothing, actual land coverage will be significantly lower than this value (e.g. 0.35 typically yields ~10-15% land coverage).")
		outPath       = flag.String("out", "", "Output file path (default: stdout)")
	)
	flag.Parse()

	actualSeed := *seed
	if actualSeed == 0 {
		actualSeed = time.Now().UnixNano()
	}
	fmt.Fprintf(os.Stderr, "seed: %d\n", actualSeed)

	var result *mapdata.GameMap
	for attempt := 0; attempt < maxAttempts; attempt++ {
		// Each attempt uses a derived seed so retries explore different terrain
		// while remaining reproducible from the original seed.
		rng := rand.New(rand.NewSource(actualSeed + int64(attempt)))

		m, err := generate(rng, *name, *desc, *islandDensity, actualSeed)
		if err != nil {
			fmt.Fprintf(os.Stderr, "attempt %d: generation failed: %v\n", attempt+1, err)
			continue
		}

		validation := mapdata.Validate(m)
		for _, w := range validation.Warnings {
			fmt.Fprintf(os.Stderr, "warning: %v\n", w)
		}
		if !validation.OK() {
			for _, e := range validation.Errors {
				fmt.Fprintf(os.Stderr, "attempt %d: %v\n", attempt+1, e)
			}
			continue
		}

		result = m
		fmt.Fprintf(os.Stderr, "map generated on attempt %d\n", attempt+1)
		break
	}

	if result == nil {
		log.Fatalf("failed to generate a valid map after %d attempts — try different parameters or a different seed", maxAttempts)
	}

	data, err := json.MarshalIndent(result, "", "  ")
	if err != nil {
		log.Fatalf("marshal: %v", err)
	}

	if *outPath == "" {
		fmt.Println(string(data))
		return
	}
	if err := os.WriteFile(*outPath, data, 0644); err != nil {
		log.Fatalf("write %q: %v", *outPath, err)
	}
	fmt.Fprintf(os.Stderr, "map written to %s\n", *outPath)
}

func generate(rng *rand.Rand, name, desc string, islandDensity float64, seed int64) (*mapdata.GameMap, error) {
	cells := mapgen.GenerateTerrain(rng, mapWidth, mapHeight, islandDensity)

	m := &mapdata.GameMap{
		Width:  mapWidth,
		Height: mapHeight,
		Cells:  cells,
		Meta: mapdata.MapMeta{
			Name:        name,
			Description: desc,
			GeneratedBy: version,
			Seed:        seed,
		},
	}

	ports, err := mapgen.PlacePorts(rng, m)
	if err != nil {
		return nil, fmt.Errorf("ports: %w", err)
	}
	m.Ports = ports

	return m, nil
}
