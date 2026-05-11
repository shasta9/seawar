package mapgen

import (
	"math/rand"

	"github.com/shasta9/seawar/pkg/mapdata"
)

// GenerateTerrain produces a land/sea grid using cellular automata.
// The pipeline is: random seed → smooth → remove tiny islands → ensure sea connectivity.
func GenerateTerrain(rng *rand.Rand, width, height int, landRatio float64) [][]mapdata.CellType {
	cells := seedNoise(rng, width, height, landRatio)
	for i := 0; i < 5; i++ {
		cells = smoothPass(cells, width, height)
	}
	cells = removeTinyIslands(cells, width, height, 4)
	cells = ensureSeaConnectivity(cells, width, height)
	return cells
}

// seedNoise fills each cell randomly according to landRatio.
func seedNoise(rng *rand.Rand, width, height int, landRatio float64) [][]mapdata.CellType {
	cells := make([][]mapdata.CellType, height)
	for y := range cells {
		cells[y] = make([]mapdata.CellType, width)
		for x := range cells[y] {
			if rng.Float64() < landRatio {
				cells[y][x] = mapdata.Land
			}
		}
	}
	return cells
}

// smoothPass applies one iteration of the cellular automata rule:
//   >= 5 land neighbours → land
//   <= 3 land neighbours → sea
//   else → unchanged
//
// Out-of-bounds neighbours count as land, which keeps coastlines off the map edges.
func smoothPass(cells [][]mapdata.CellType, width, height int) [][]mapdata.CellType {
	next := make([][]mapdata.CellType, height)
	for y := range next {
		next[y] = make([]mapdata.CellType, width)
		for x := range next[y] {
			n := countLandNeighbours(cells, x, y, width, height)
			if n >= 5 {
				next[y][x] = mapdata.Land
			} else if n <= 3 {
				next[y][x] = mapdata.Sea
			} else {
				next[y][x] = cells[y][x]
			}
		}
	}
	return next
}

// countLandNeighbours counts land cells in the Moore neighbourhood (8 surrounding cells).
// Cells outside the map boundary are treated as land.
func countLandNeighbours(cells [][]mapdata.CellType, x, y, width, height int) int {
	count := 0
	for dy := -1; dy <= 1; dy++ {
		for dx := -1; dx <= 1; dx++ {
			if dx == 0 && dy == 0 {
				continue
			}
			nx, ny := x+dx, y+dy
			if nx < 0 || nx >= width || ny < 0 || ny >= height {
				count++ // boundary is treated as land
			} else if cells[ny][nx] == mapdata.Land {
				count++
			}
		}
	}
	return count
}

// removeTinyIslands converts any landmass smaller than minSize cells to sea.
func removeTinyIslands(cells [][]mapdata.CellType, width, height, minSize int) [][]mapdata.CellType {
	visited := make([][]bool, height)
	for y := range visited {
		visited[y] = make([]bool, width)
	}
	for y := 0; y < height; y++ {
		for x := 0; x < width; x++ {
			if cells[y][x] == mapdata.Land && !visited[y][x] {
				region := bfsLand(cells, visited, width, height, mapdata.Position{X: x, Y: y})
				if len(region) < minSize {
					for _, pos := range region {
						cells[pos.Y][pos.X] = mapdata.Sea
					}
				}
			}
		}
	}
	return cells
}

func bfsLand(cells [][]mapdata.CellType, visited [][]bool, width, height int, start mapdata.Position) []mapdata.Position {
	var result []mapdata.Position
	queue := []mapdata.Position{start}
	visited[start.Y][start.X] = true
	for len(queue) > 0 {
		cur := queue[0]
		queue = queue[1:]
		result = append(result, cur)
		for _, d := range orthoDirs {
			n := mapdata.Position{X: cur.X + d.X, Y: cur.Y + d.Y}
			if InBounds(n, width, height) && cells[n.Y][n.X] == mapdata.Land && !visited[n.Y][n.X] {
				visited[n.Y][n.X] = true
				queue = append(queue, n)
			}
		}
	}
	return result
}

// ensureSeaConnectivity merges disconnected sea regions by carving corridors through land.
// It repeatedly finds the two closest disconnected sea regions and opens a straight-line
// corridor between them until only one sea region remains.
func ensureSeaConnectivity(cells [][]mapdata.CellType, width, height int) [][]mapdata.CellType {
	for {
		regions := ConnectedSeaRegions(cells, width, height)
		if len(regions) <= 1 {
			break
		}

		// Find the largest region.
		largest := 0
		for i, r := range regions {
			if len(r) > len(regions[largest]) {
				largest = i
			}
		}

		// Find the closest cell pair between the largest region and any other region.
		var bestA, bestB mapdata.Position
		bestDist := width * height
		for i, region := range regions {
			if i == largest {
				continue
			}
			for _, a := range regions[largest] {
				for _, b := range region {
					if d := ManhattanDist(a, b); d < bestDist {
						bestDist = d
						bestA, bestB = a, b
					}
				}
			}
		}

		// Carve a corridor from bestB toward bestA (axis-aligned, horizontal then vertical).
		carveCorridor(cells, bestB, bestA)
	}
	return cells
}

// carveCorridor opens a sea path from src to dst by converting land cells to sea.
// It walks horizontally first, then vertically.
func carveCorridor(cells [][]mapdata.CellType, src, dst mapdata.Position) {
	x, y := src.X, src.Y
	for x != dst.X {
		cells[y][x] = mapdata.Sea
		if dst.X > x {
			x++
		} else {
			x--
		}
	}
	for y != dst.Y {
		cells[y][x] = mapdata.Sea
		if dst.Y > y {
			y++
		} else {
			y--
		}
	}
}
