// solver.cpp

#include <iostream>
#include "solver.h"
#include <omp.h>

Solver::Solver() {
    for (int y = 0; y < 9; ++y) {
        for (int x = 0; x < 9; ++x) {
            data[y][x] = 0;
        }
    }
}

// Impements the Gordon Royle's input format and
// "SDK" format from G.Ralph Kuntz https://github.com/grkuntzmd/go-sudoku
Solver::Solver(const char* init) {
    for (int i = 0; i < 81; ++i) {
        int x = i % 9;
        int y = i / 9;
	if (init[i] < '0' || init[i] > '9')
            data[y][x] = 0;
	else 
            data[y][x] = init[i] - '0';
    }
}

Solver::Solver(const Solver* init) {
    for (int y = 0; y < 9; ++y) {
        for (int x = 0; x < 9; ++x) {
            data[y][x] = init->data[y][x];
        }
    }
}

void Solver::print(std::ostream & s) {
    for (int y = 0; y < 9; ++y) {
        for (int x = 0; x < 9; ++x) {
            s << (char)(data[y][x] + '0') << " ";
        }
        s << std::endl;
    }
}

bool Solver::isSolved() {
    // Check whether every cell is filled in the table?
    for (int y = 0; y < 9; ++y) {
        for (int x = 0; x < 9; ++x) {
            if (data[y][x] == 0) return false;
        }
    }
    return true;
}

bool Solver::isAllowed(char val, int x, int y) {
    bool allowed = true;

    // Only one 'val' is allowed in same row and column
    for (int i = 0; i < 9; ++i) {
        if (data[y][i] == val) allowed = false;
        if (data[i][x] == val) allowed = false;
    }

    // Only one 'val' is allowed in a 3x3 cell
    int cellBaseX = 3 * (int)(x / 3);
    int cellBaseY = 3 * (int)(y / 3);
    for (int y = cellBaseY; y < cellBaseY + 3; ++y) {
        for (int x = cellBaseX; x < cellBaseX + 3; ++x) {
            if (data[y][x] == val) allowed = false;
        }
    }
    return allowed;
}

int Solver::solveBackTrack(int depth) {
    // Are we finished?
    if (isSolved()) {
        #pragma omp critical
        {
            std::cout << "Solution found:\n";
            print(std::cout);
        }
        return 1; 
    }

    // Find an empty cell 
    for (int y = 0; y < 9; ++y) {
        for (int x = 0; x < 9; ++x) {
            if (data[y][x] == 0) {
                int solutions = 0;
                // Find an appropriate 'val'
                for (int n = 1; n <= 9; ++n) {
                    if (isAllowed(n, x, y)) {
                        // Create OpenMP tasks for search branches.
                        // depth < 4 prevents creating too many small tasks overhead.
                        #pragma omp task shared(solutions) if(depth < 4)
                        {
                            Solver tmpSolver(this);
                            tmpSolver.set(n, x, y);
                            int sub_solutions = tmpSolver.solveBackTrack(depth + 1);
                            
                            // Atomic operation for thread-safe addition
                            #pragma omp atomic
                            solutions += sub_solutions;
                        }
                    }
                }
                // Wait for all parallel branches at this level to finish
                #pragma omp taskwait
                
                return solutions;
            }
        }
    }
    return 0; // No solution on this branch
}

void Solver::set(char val, int x, int y) {
    data[y][x] = val;
}
