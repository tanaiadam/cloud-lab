#!/bin/bash
#SBATCH -o fractal.out
#SBATCH --nodes=1
#SBATCH --tasks-per-node=1
#SBATCH --cpus-per-task=4  

export OMP_NUM_THREADS=$SLURM_CPUS_PER_TASK
./fractal