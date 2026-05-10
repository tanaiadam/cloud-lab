#include <iostream>
#include <chrono>
#include <omp.h>
#include "solver.h"

int main(int argc, const char* argv[]) {
    int start_idx = (argc == 1) ? 0 : 1;
    const char *test = "000801000000000043500000000000070800020030000000000100600000075003400000000200600";
 
    auto start = std::chrono::high_resolution_clock::now();

    for (int i = start_idx; i < argc; i++) {
        const char *t = (i == 0) ? test : argv[i];
        Solver solver(t);
        
        int problem_num = (i == 0) ? 1 : i;
        std::cout << "Problem #" << problem_num << ":\n";
        solver.print(std::cout); // Print initial board
        
        int solutions = 0;

        #pragma omp parallel
        {
            #pragma omp single
            {
                solutions = solver.solveBackTrack(0);
            }
        }
        
        if (solutions == 0) {
            std::cout << "Solutions #" << problem_num << ": 'Cannot solve problem'\n";
        } else {
            std::cout << "Solutions: " << solutions << "\n";
        }
        std::cout << "-----------------\n";
    }

    auto end = std::chrono::high_resolution_clock::now();
    std::chrono::duration<double> diff = end - start;

    std::cout << "total elapsed time: " << diff.count() << " s\n";
    
    return 0;
}