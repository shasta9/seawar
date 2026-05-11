package mapgen

import "github.com/shasta9/seawar/pkg/mapdata"

// orthoDirs are the four cardinal directions used for connectivity checks.
var orthoDirs = []mapdata.Position{
	{X: 0, Y: -1},
	{X: 0, Y: 1},
	{X: -1, Y: 0},
	{X: 1, Y: 0},
}

// InBounds reports whether pos lies within a grid of the given dimensions.
func InBounds(pos mapdata.Position, width, height int) bool {
	return pos.X >= 0 && pos.X < width && pos.Y >= 0 && pos.Y < height
}

// ManhattanDist returns the Manhattan distance between two positions.
func ManhattanDist(a, b mapdata.Position) int {
	dx := a.X - b.X
	if dx < 0 {
		dx = -dx
	}
	dy := a.Y - b.Y
	if dy < 0 {
		dy = -dy
	}
	return dx + dy
}

// ChebyshevDist returns the Chebyshev (chessboard) distance between two positions.
func ChebyshevDist(a, b mapdata.Position) int {
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

// ConnectedSeaRegions returns all connected sea regions as slices of positions.
// Each inner slice is one contiguous sea region.
func ConnectedSeaRegions(cells [][]mapdata.CellType, width, height int) [][]mapdata.Position {
	visited := make([][]bool, height)
	for y := range visited {
		visited[y] = make([]bool, width)
	}

	var regions [][]mapdata.Position
	for y := 0; y < height; y++ {
		for x := 0; x < width; x++ {
			if cells[y][x] == mapdata.Sea && !visited[y][x] {
				region := bfsSea(cells, visited, width, height, mapdata.Position{X: x, Y: y})
				regions = append(regions, region)
			}
		}
	}
	return regions
}

// FloodFillSea returns all sea cells reachable from start via orthogonal moves.
func FloodFillSea(cells [][]mapdata.CellType, width, height int, start mapdata.Position) []mapdata.Position {
	visited := make([][]bool, height)
	for y := range visited {
		visited[y] = make([]bool, width)
	}
	return bfsSea(cells, visited, width, height, start)
}

func bfsSea(cells [][]mapdata.CellType, visited [][]bool, width, height int, start mapdata.Position) []mapdata.Position {
	var result []mapdata.Position
	queue := []mapdata.Position{start}
	visited[start.Y][start.X] = true
	for len(queue) > 0 {
		cur := queue[0]
		queue = queue[1:]
		result = append(result, cur)
		for _, d := range orthoDirs {
			n := mapdata.Position{X: cur.X + d.X, Y: cur.Y + d.Y}
			if InBounds(n, width, height) && cells[n.Y][n.X] == mapdata.Sea && !visited[n.Y][n.X] {
				visited[n.Y][n.X] = true
				queue = append(queue, n)
			}
		}
	}
	return result
}
