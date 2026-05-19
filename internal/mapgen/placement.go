package mapgen

import (
	"fmt"
	"math/rand"

	"github.com/shasta9/seawar/pkg/mapdata"
)

const minPortSeparation = 12

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
