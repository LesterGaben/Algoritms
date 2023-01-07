import numpy as np


class Ant:
    def __init__(self, graph_matrix):
        self.graph_matrix = graph_matrix
        self.position = np.random.randint(len(graph_matrix))
        self.allowed = list(range(len(graph_matrix)))
        self.taboo = [self.position]
        self.allowed.remove(self.position)

    def restart(self):
        self.position = np.random.randint(len(self.graph_matrix))
        self.allowed = list(range(len(self.graph_matrix)))
        self.taboo = [self.position]
        self.allowed.remove(self.position)

    def add_pheromone(self):
        Q = 10
        L = 0
        prev = self.taboo[0]
        for i in self.taboo[1:]:
            L += len(self.graph_matrix[prev, i])
            prev = i
        L += len(self.graph_matrix[self.taboo[0], prev])
        pheromone_add = Q / L
        prev = self.taboo[0]
        for j in self.taboo[1:]:
            self.graph_matrix[prev, j].pheromone += pheromone_add
            self.graph_matrix[j, prev].pheromone += pheromone_add
            prev = j
        self.graph_matrix[self.taboo[0], prev].pheromone += pheromone_add
        self.graph_matrix[prev, self.taboo[0]].pheromone += pheromone_add

    def run(self):
        while True:
            if len(self.allowed) == 0:
                if self.graph_matrix[self.taboo[0], self.taboo[-1]] == 0:
                    self.position = np.random.randint(len(self.graph_matrix))
                    self.allowed = list(range(len(self.graph_matrix)))
                    self.taboo = [self.position]
                    self.allowed.remove(self.position)
                    continue
                else:
                    break
            elif not self.graph_matrix[self.position][self.allowed].any():
                self.position = np.random.randint(len(self.graph_matrix))
                self.allowed = list(range(len(self.graph_matrix)))
                self.taboo = [self.position]
                self.allowed.remove(self.position)
                continue
            self.step()

    def step(self):
        r = np.random.rand()
        desire_list = self.want()
        for i in range(len(desire_list)):
            if r < desire_list[i]:
                vortex = self.allowed[i]
                self.allowed.remove(vortex)
                self.taboo.append(vortex)
                self.position = vortex
                break
            else:
                r -= desire_list[i]

    def want(self):
        desire_list = []
        for index in self.allowed:
            desire_list.append(self.graph_matrix[self.position][index].desire_to_walk())
        if len(desire_list) == 0:
            desire_sum = sum(desire_list)
        else:
            desire_sum = sum(desire_list)
        for i in range(len(desire_list)):
            try:
                desire_list[i] /= desire_sum
            except ZeroDivisionError:
                print(desire_list, self.allowed)
        return desire_list


class Edge:
    def __init__(self, length):
        self.length = length
        self.pheromone = 1

    def __len__(self):
        return self.length

    def __str__(self):
        return f"{self.length} {self.pheromone}"

    def __repr__(self):
        return str(self)

    def desire_to_walk(self, alfa=2, beta=4):
        if self.length == 0:
            return 0
        return (self.pheromone**alfa) * ((10/self.length)**beta)

    def run_time(self, p=0.6):
        self.pheromone *= p


class Graph:
    def __init__(self, size, min_edge_weight=0, max_edge_weight=51):
        matrix = np.random.randint(min_edge_weight, max_edge_weight, size=[size, size])
        for i in range(len(matrix)):
            matrix[i:] = matrix.T[i:]
        self.matrix = np.array([[Edge(length) for length in row] for row in matrix])

    def run_time(self):
        for row in self.matrix:
            for edge in row:
                edge.run_time()

    def ant_algorithm(self, number_of_ants):
        ants = [Ant(self.matrix) for i in range(number_of_ants)]
        for i in range(1001):
            for r in ants:
                r.restart()
            for a in ants:
                a.run()
            for ant in ants:
                ant.add_pheromone()
            for row in self.matrix:
                for element in row:
                    element.run_time()
            if i % 20 == 0:
                print(self.get_best_len())
        return self.get_best_len()

    def get_best(self):
        allowed = list(range(1, len(self.matrix)))
        taboo = [0]
        for i in range(len(allowed)):
            next_vortex = max(allowed, key=lambda x: self.matrix[taboo[-1], x].pheromone)
            allowed.remove(next_vortex)
            taboo.append(next_vortex)
        return taboo

    def get_best_len(self):
        best_line = self.get_best()
        L = 0
        prev = best_line[0]
        for i in best_line[1:]:
            L += len(self.matrix[prev, i])
            prev = i
        L += len(self.matrix[best_line[0], prev])
        return L


def main():
    g = Graph(250)
    g.ant_algorithm(45)


if __name__ == "__main__":
    main()
