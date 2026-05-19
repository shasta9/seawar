package mapdata

// CellType distinguishes land from sea.
type CellType uint8

const (
	Sea  CellType = 0
	Land CellType = 1
)

// PortOwner identifies which player a port belongs to.
type PortOwner uint8

const (
	SubPort       PortOwner = 1
	DestroyerPort PortOwner = 2
)

// Position is a grid coordinate. (0,0) is top-left; X increases east, Y increases south.
type Position struct {
	X int `json:"x"`
	Y int `json:"y"`
}

// Port is a named map location with an owner.
type Port struct {
	Name  string    `json:"name"`
	Pos   Position  `json:"pos"`
	Owner PortOwner `json:"owner"`
}

// MapMeta carries human-readable provenance information.
type MapMeta struct {
	Name        string `json:"name"`
	Description string `json:"description,omitempty"`
	GeneratedBy string `json:"generated_by"`
	Seed        int64  `json:"seed"`
}

// GameMap is the complete authoritative map loaded by the server.
// Cells are stored as a [Height][Width] grid; Cells[Y][X] is the cell at column X, row Y.
// The JSON representation encodes each row as a string of '0' (sea) and '1' (land) characters.
type GameMap struct {
	Width  int          `json:"-"`
	Height int          `json:"-"`
	Cells  [][]CellType `json:"-"`
	Ports  []Port       `json:"ports"`
	Meta   MapMeta      `json:"meta"`
}

// InBounds reports whether pos lies within the map grid.
func (m *GameMap) InBounds(pos Position) bool {
	return pos.X >= 0 && pos.X < m.Width && pos.Y >= 0 && pos.Y < m.Height
}
