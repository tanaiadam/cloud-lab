// solver.h

#pragma once

class Solver {
public:
    Solver();
    Solver(const char* init);
    Solver(const Solver* init);

    void print(std::ostream &s);
    bool isSolved();
    bool isAllowed(char val, int x, int y);
    int solveBackTrack(int depth = 0);
    void set(char val, int x, int y);
private:
    char data[9][9];
};

