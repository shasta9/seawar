package mapdata

import (
	"encoding/json"
	"fmt"
	"strings"
)

// gameMapWire is the JSON wire format. It mirrors GameMap but encodes cells as row strings.
type gameMapWire struct {
	Width     int            `json:"width"`
	Height    int            `json:"height"`
	Cells     []string       `json:"cells"`
	Ports     []Port         `json:"ports"`
	Merchants []MerchantShip `json:"merchants"`
	Start     StartPositions `json:"start"`
	Meta      MapMeta        `json:"meta"`
}

func (m GameMap) MarshalJSON() ([]byte, error) {
	rows := make([]string, m.Height)
	for y, row := range m.Cells {
		var sb strings.Builder
		sb.Grow(m.Width)
		for _, cell := range row {
			if cell == Land {
				sb.WriteByte('1')
			} else {
				sb.WriteByte('0')
			}
		}
		rows[y] = sb.String()
	}
	wire := gameMapWire{
		Width:     m.Width,
		Height:    m.Height,
		Cells:     rows,
		Ports:     m.Ports,
		Merchants: m.Merchants,
		Start:     m.Start,
		Meta:      m.Meta,
	}
	return json.Marshal(wire)
}

func (m *GameMap) UnmarshalJSON(data []byte) error {
	var wire gameMapWire
	if err := json.Unmarshal(data, &wire); err != nil {
		return err
	}
	if len(wire.Cells) != wire.Height {
		return fmt.Errorf("mapdata: cells row count %d does not match height %d", len(wire.Cells), wire.Height)
	}
	cells := make([][]CellType, wire.Height)
	for y, row := range wire.Cells {
		if len(row) != wire.Width {
			return fmt.Errorf("mapdata: row %d has length %d, expected %d", y, len(row), wire.Width)
		}
		cells[y] = make([]CellType, wire.Width)
		for x, ch := range row {
			switch ch {
			case '0':
				cells[y][x] = Sea
			case '1':
				cells[y][x] = Land
			default:
				return fmt.Errorf("mapdata: invalid cell character %q at (%d,%d)", ch, x, y)
			}
		}
	}
	m.Width = wire.Width
	m.Height = wire.Height
	m.Cells = cells
	m.Ports = wire.Ports
	m.Merchants = wire.Merchants
	m.Start = wire.Start
	m.Meta = wire.Meta
	return nil
}
