# 
# Example Makefile to compile and run
#

# compile
# should be modified according to the tools of the paralellization
all: fractal.cpp
	g++ -O3 -fopenmp -o fractal fractal.cpp

# run on the cluster
# should be also modified
.phony:
run:	fractal
	srun -o fractal.out ./fractal
	cat fractal.out

# run on the cluster
# should be also modified
.phony:
batch:	fractal
	sbatch fractal.sh

.phony:
clean:
	rm -rf fractal mandelbrot.tga *.out


