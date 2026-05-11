package mapdata

import "fmt"

// ValidationError describes a single failed check.
type ValidationError struct {
	Check   string
	Message string
}

func (e ValidationError) Error() string {
	return fmt.Sprintf("%s: %s", e.Check, e.Message)
}

// ValidationResult collects errors and warnings from a Validate call.
// Errors indicate the map is unplayable. Warnings are advisory.
type ValidationResult struct {
	Errors   []ValidationError
	Warnings []ValidationError
}

func (r *ValidationResult) addError(check, msg string) {
	r.Errors = append(r.Errors, ValidationError{Check: check, Message: msg})
}

func (r *ValidationResult) addWarning(check, msg string) {
	r.Warnings = append(r.Warnings, ValidationError{Check: check, Message: msg})
}

// OK returns true if there are no errors (warnings do not affect this).
func (r *ValidationResult) OK() bool {
	return len(r.Errors) == 0
}

var orthoDirs = []Position{{0, -1}, {0, 1}, {-1, 0}, {1, 0}}

// Validate runs all validation checks on m. It is called by both the generator
// and the server when loading a map file.
func Validate(m *GameMap) ValidationResult {
	var result ValidationResult

	reachable := seaRegionIDs(m)

	// --- Ports ---
	for i, p := range m.Ports {
		if !m.InBounds(p.Pos) {
			result.addError("port-bounds",
				fmt.Sprintf("port %d %q at %v is out of bounds", i, p.Name, p.Pos))
			continue
		}
		if m.Cells[p.Pos.Y][p.Pos.X] != Sea {
			result.addError("port-on-sea",
				fmt.Sprintf("port %d %q at %v is on land", i, p.Name, p.Pos))
			continue
		}
		if !isCoastal(m, p.Pos) {
			result.addError("port-coastal",
				fmt.Sprintf("port %d %q at %v is not adjacent to land", i, p.Name, p.Pos))
		}
	}

	// All ports must be in the same sea region (reachable from each other).
	if len(m.Ports) > 1 {
		ref := m.Ports[0]
		if m.InBounds(ref.Pos) && m.Cells[ref.Pos.Y][ref.Pos.X] == Sea {
			refID := reachable[ref.Pos.Y][ref.Pos.X]
			for i := 1; i < len(m.Ports); i++ {
				p := m.Ports[i]
				if m.InBounds(p.Pos) && m.Cells[p.Pos.Y][p.Pos.X] == Sea {
					if reachable[p.Pos.Y][p.Pos.X] != refID {
						result.addError("port-connectivity",
							fmt.Sprintf("port %d %q is not reachable from port 0 %q", i, p.Name, ref.Name))
					}
				}
			}
		}
	}

	// Minimum separation between any pair of ports.
	const minPortSep = 12
	for i := 0; i < len(m.Ports); i++ {
		for j := i + 1; j < len(m.Ports); j++ {
			d := chebyshevDist(m.Ports[i].Pos, m.Ports[j].Pos)
			if d < minPortSep {
				result.addError("port-separation",
					fmt.Sprintf("ports %d and %d are only %d cells apart (minimum %d)", i, j, d, minPortSep))
			}
		}
	}

	// --- Starting positions ---
	for _, sp := range []struct {
		name string
		pos  Position
	}{
		{"sub", m.Start.Sub},
		{"destroyer", m.Start.Destroyer},
	} {
		if !m.InBounds(sp.pos) {
			result.addError("start-bounds",
				fmt.Sprintf("%s start %v is out of bounds", sp.name, sp.pos))
			continue
		}
		if m.Cells[sp.pos.Y][sp.pos.X] != Sea {
			result.addError("start-on-sea",
				fmt.Sprintf("%s start %v is on land", sp.name, sp.pos))
		}
	}

	const minStartSep = 20
	if d := chebyshevDist(m.Start.Sub, m.Start.Destroyer); d < minStartSep {
		result.addError("start-separation",
			fmt.Sprintf("starting positions are only %d cells apart (minimum %d)", d, minStartSep))
	}

	// --- Merchant ships ---
	for i, ms := range m.Merchants {
		if !m.InBounds(ms.Pos) {
			result.addError("merchant-bounds",
				fmt.Sprintf("merchant %d at %v is out of bounds", i, ms.Pos))
			continue
		}
		if m.Cells[ms.Pos.Y][ms.Pos.X] != Sea {
			result.addError("merchant-on-sea",
				fmt.Sprintf("merchant %d at %v is on land", i, ms.Pos))
		}
	}

	// --- Advisory warnings ---
	landCount := 0
	total := m.Width * m.Height
	for y := 0; y < m.Height; y++ {
		for x := 0; x < m.Width; x++ {
			if m.Cells[y][x] == Land {
				landCount++
			}
		}
	}
	ratio := float64(landCount) / float64(total)
	if ratio < 0.10 || ratio > 0.70 {
		result.addWarning("land-ratio",
			fmt.Sprintf("land ratio %.1f%% is unusually extreme", ratio*100))
	}

	return result
}

// isCoastal reports whether a sea cell has at least one orthogonal land neighbour.
func isCoastal(m *GameMap, pos Position) bool {
	for _, d := range orthoDirs {
		n := Position{pos.X + d.X, pos.Y + d.Y}
		if m.InBounds(n) && m.Cells[n.Y][n.X] == Land {
			return true
		}
	}
	return false
}

// chebyshevDist is the chessboard distance between two positions.
func chebyshevDist(a, b Position) int {
	dx := a.X - b.X
	if dx < 0 {
		dx = -dx
	}
	dy := a.Y - b.Y
	if dy < 0 {
		dy = -dy
	}
	if dx > dy {
		return dx
	}
	return dy
}

// seaRegionIDs returns a grid where each sea cell holds an integer region ID.
// Cells in the same connected sea region share the same ID. Land cells hold -1.
func seaRegionIDs(m *GameMap) [][]int {
	ids := make([][]int, m.Height)
	for y := range ids {
		ids[y] = make([]int, m.Width)
		for x := range ids[y] {
			ids[y][x] = -1
		}
	}
	regionID := 0
	for y := 0; y < m.Height; y++ {
		for x := 0; x < m.Width; x++ {
			if m.Cells[y][x] == Sea && ids[y][x] == -1 {
				bfsLabel(m, ids, Position{x, y}, regionID)
				regionID++
			}
		}
	}
	return ids
}

func bfsLabel(m *GameMap, ids [][]int, start Position, id int) {
	queue := []Position{start}
	ids[start.Y][start.X] = id
	for len(queue) > 0 {
		cur := queue[0]
		queue = queue[1:]
		for _, d := range orthoDirs {
			n := Position{cur.X + d.X, cur.Y + d.Y}
			if m.InBounds(n) && m.Cells[n.Y][n.X] == Sea && ids[n.Y][n.X] == -1 {
				ids[n.Y][n.X] = id
				queue = append(queue, n)
			}
		}
	}
}
