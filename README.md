# Flow fields

## Description of the topic
**Flow fields** are used in games to reduce the computational power spent on pathfinding in case of large crowds, which you can find in Real Time Strategy (RTS) games where you have large armies or some tycoon games like Planet Coaster.

<img src="https://github.com/Wardergrip/GPP_Research/blob/main/Documentation/Demo.gif" alt="A gif depicting a flow field that gets a new target in realtime" width="600">

## Design/Implementation  

The idea, in essence, is to create a grid. Every cell of this grid will tell you a direction on where to go to get closer to the goal. AI agents will only have to check which cell they occupy to know which direction to go.

Easier said than done; let's see how we tackle this problem.

We will split the problem into 3 parts:
* Cost field
* Integration field
* Flow field

The cost field essentially describes how hard it is to traverse each cell. By default, a cell costs `1` to traverse and impassable terrain costs `2147483647`, which is `int.MaxValue`. Regarding gameplay, a higher cost would mean slower movement for the agent.

<img src="https://github.com/Wardergrip/GPP_Research/blob/main/Documentation/CostField.png" alt="A grid with numbers. Each number represents the cost of that cell" width="700"/>

The integration field describes the `totalCost` for each cell to reach the goal node. This part is where all the magic happens. More info on this later.

<img src="https://github.com/Wardergrip/GPP_Research/blob/main/Documentation/IntegrationField.png" alt="A grid with numbers. Each number represents the total cost of that cell" width="700"/>

The flow field capitalizes on the values calculated by the integration field. For each cell, it looks for the lowest value neighboring it and sets its direction to that cell.
Even for impassable terrain, a direction is calculated. Bugs happen, and if an agent get's stuck into impassable terrain, it won't get stuck forever.

<img src="https://github.com/Wardergrip/GPP_Research/blob/main/Documentation/FlowField.png" alt="A grid with arrows. Each arrow points to the lowest totalCost neighbor" width="700"/>

### Integration field

We start by setting each cell's `totalCost` to `int.MaxValue`. If, at the end of our algorithm, the `totalCost` is still `int.MaxValue`, no path is found from that cell to the target cell.

The rest of the algorithm is based on [Dijkstra's algorithm ](https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm).
We make an `openList` which serves as a `queue` and a `processedList` to ensure we don't end up in an endlessly repeating cycle. 
We add our target node to the `openList` and set it's `totalCost` to `0`.  

As long as the `openList` is not empty, we get the first cell in the `openList` and check for each neighbour of that cell if `neighbour.totalCost > currentCell.totalCost + neighbour.cost`. If this is the case, it means we found a better way to get to this cell, and we replace the `totalCost` with `currentCell.totalCost + neighbour.cost`. If the cell is impassable, we don't change anything about the `totalCost` at all.
After that, we can add the neighbor to the `openList` if it isn't already there.

We flag this cell as processed and remove it from our `openList`.

Once the `openList` is empty, each cell's `totalCost` will describe how "long" or "expensive" it is to get to the target cell.


## Result  

https://user-images.githubusercontent.com/42802496/212024349-35e481c2-7ca2-4c63-81e8-a5345e596771.mp4

## Conclusion / Future work
