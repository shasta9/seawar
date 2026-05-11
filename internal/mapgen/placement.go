package mapgen

import (
	"fmt"
	"math/rand"

	"github.com/shasta9/seawar/pkg/mapdata"
)

const (
	minPortSeparation    = 12
	minMerchantSep       = 4
	minMerchantPortSep   = 6
	minStartSeparation   = 20
)

// PlacePorts places 4 ports on coastal sea cells:
//   - 2 sub ports in the southern half (rows height/2 .. height-1)
//   - 2 destroyer ports in the northern half (rows 0 .. height/2-1)
//
// All ports are separated by at least minPortSeparation cells (Chebyshev distance).
func PlacePorts(rng *rand.Rand, m *mapdata.GameMap) ([]mapdata.Port, error) {
	midY := m.Height / 2
	ports := make([]mapdata.Port, 0, 4)

	type portSpec struct {
		name  string
		owner mapdata.PortOwner
		y0    int
		y1    int
	}
	specs := []portSpec{
		{"Port Alpha", mapdata.SubPort, midY, m.Height - 1},
		{"Port Beta", mapdata.SubPort, midY, m.Height - 1},
		{"Port Gamma", mapdata.DestroyerPort, 0, midY - 1},
		{"Port Delta", mapdata.DestroyerPort, 0, midY - 1},
	}

	for _, spec := range specs {
		pos, err := findCoastalCell(rng, m, 0, m.Width-1, spec.y0, spec.y1, ports)
		if err != nil {
			return nil, fmt.Errorf("placing %q: %w", spec.name, err)
		}
		ports = append(ports, mapdata.Port{
			Name:  spec.name,
			Pos:   pos,
			Owner: spec.owner,
		})
	}
	return ports, nil
}

// PlaceMerchants places n merchant ships on sea cells, respecting minimum separation
// from each other and from ports.
func PlaceMerchants(rng *rand.Rand, m *mapdata.GameMap, n int) ([]mapdata.MerchantShip, error) {
	merchants := make([]mapdata.MerchantShip, 0, n)
	for i := 0; i < n; i++ {
		pos, err := findSeaCell(rng, m, 0, m.Width-1, 0, m.Height-1, func(p mapdata.Position) bool {
			for _, port := range m.Ports {
				if ChebyshevDist(p, port.Pos) < minMerchantPortSep {
					return false
				}
			}
			for _, ms := range merchants {
				if ChebyshevDist(p, ms.Pos) < minMerchantSep {
					return false
				}
			}
			return true
		})
		if err != nil {
			return nil, fmt.Errorf("merchant %d: %w", i, err)
		}
		merchants = append(merchants, mapdata.MerchantShip{Pos: pos})
	}
	return merchants, nil
}

// PlaceStartPositions places the submarine start in the southern half and the
// destroyer start in the northern half, at least minStartSeparation cells apart.
func PlaceStartPositions(rng *rand.Rand, m *mapdata.GameMap) (mapdata.StartPositions, error) {
	midY := m.Height / 2

	subPos, err := findSeaCell(rng, m, 0, m.Width-1, midY, m.Height-1, func(p mapdata.Position) bool {
		return true
	})
	if err != nil {
		return mapdata.StartPositions{}, fmt.Errorf("sub start: %w", err)
	}

	destroyerPos, err := findSeaCell(rng, m, 0, m.Width-1, 0, midY-1, func(p mapdata.Position) bool {
		return ChebyshevDist(p, subPos) >= minStartSeparation
	})
	if err != nil {
		return mapdata.StartPositions{}, fmt.Errorf("destroyer start: %w", err)
	}

	return mapdata.StartPositions{Sub: subPos, Destroyer: destroyerPos}, nil
}

// findCoastalCell finds a random coastal sea cell within [x0,x1]×[y0,y1] that is
// at least minPortSeparation cells from every port already in existing.
func findCoastalCell(rng *rand.Rand, m *mapdata.GameMap, x0, x1, y0, y1 int, existing []mapdata.Port) (mapdata.Position, error) {
	var candidates []mapdata.Position
	for y := y0; y <= y1; y++ {
		for x := x0; x <= x1; x++ {
			pos := mapdata.Position{X: x, Y: y}
			if m.Cells[y][x] == mapdata.Sea && isCoastal(m, pos) {
				candidates = append(candidates, pos)
			}
		}
	}
	if len(candidates) == 0 {
		return mapdata.Position{}, fmt.Errorf("no coastal sea cells in region (%d,%d)-(%d,%d)", x0, y0, x1, y1)
	}

	rng.Shuffle(len(candidates), func(i, j int) {
		candidates[i], candidates[j] = candidates[j], candidates[i]
	})

	for _, pos := range candidates {
		if meetsPortSeparation(pos, existing) {
			return pos, nil
		}
	}
	return mapdata.Position{}, fmt.Errorf("no coastal cell with required port separation in region (%d,%d)-(%d,%d)", x0, y0, x1, y1)
}

// findSeaCell finds a random sea cell within [x0,x1]×[y0,y1] that satisfies ok.
func findSeaCell(rng *rand.Rand, m *mapdata.GameMap, x0, x1, y0, y1 int, ok func(mapdata.Position) bool) (mapdata.Position, error) {
	var candidates []mapdata.Position
	for y := y0; y <= y1; y++ {
		for x := x0; x <= x1; x++ {
			pos := mapdata.Position{X: x, Y: y}
			if m.Cells[y][x] == mapdata.Sea && ok(pos) {
				candidates = append(candidates, pos)
			}
		}
	}
	if len(candidates) == 0 {
		return mapdata.Position{}, fmt.Errorf("no valid sea cell in region (%d,%d)-(%d,%d)", x0, y0, x1, y1)
	}
	return candidates[rng.Intn(len(candidates))], nil
}

func meetsPortSeparation(pos mapdata.Position, ports []mapdata.Port) bool {
	for _, p := range ports {
		if ChebyshevDist(pos, p.Pos) < minPortSeparation {
			return false
		}
	}
	return true
}

// isCoastal reports whether a sea cell has at least one orthogonal land neighbour.
func isCoastal(m *mapdata.GameMap, pos mapdata.Position) bool {
	for _, d := range orthoDirs {
		n := mapdata.Position{X: pos.X + d.X, Y: pos.Y + d.Y}
		if m.InBounds(n) && m.Cells[n.Y][n.X] == mapdata.Land {
			return true
		}
	}
	return false
}
