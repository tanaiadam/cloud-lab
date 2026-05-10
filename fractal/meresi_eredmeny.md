# Mérési Dokumentáció: Fraktálgenerátor Párhuzamosítása

## 1. Célkitűzés
A feladatom célja a Mandelbrot-halmaz generáló program párhuzamosítása volt OpenMP technológia segítségével, valamint a teljesítmény mérése a Komondor szupergépen.

## 2. Módszertan
- **Párhuzamosítás:** OpenMP direktívák használatával végeztem el a ciklusok párhuzamosítását (`#pragma omp parallel for`).
- **Ütemezés:** Dinamikus ütemezést (`schedule(dynamic)`) alkalmaztam, mivel a Mandelbrot-halmaz egyes pontjainak kiszámítási ideje nem egyenletes, így a dinamikus kiosztással jobb terheléselosztást értem el.
- **Fordítás:** A fordítást `g++` fordítóval végeztem, `-O3` optimalizációs szint és `-fopenmp` kapcsoló mellett.

## 3. Mérési Eredmények
A méréseim során a szálak számát 1 és 4 között változtattam a SLURM ütemezőn keresztül (`--cpus-per-task`).

| Magok száma | Futási idő [s] | Gyorsulás | Hatékonyság |
|:---:|:---:|:---:|:---:|
| 1 | 1.802 | 1.00 | 100.0% |
| 2 | 0.892 | 2.02 | 101.0% |
| 3 | 0.585 | 3.08 | 102.7% |
| 4 | 0.442 | 4.08 | 101.9% |

## 4. Elemzés és Interpretáció
- **Gyorsulás (Speedup):** A méréseim majdnem lineáris gyorsulást mutatnak. 4 mag esetén 4.08-szeres gyorsulást tapasztaltam, amit rendkívül jó eredménynek tartok.
- **Szuperlineáris gyorsulás:** Megfigyeltem, hogy a hatékonyság esetenként 100% feletti. Ezt azzal magyarázom, hogy az adatok jobban elférnek a processzormagok saját cache memóriájában, mint a soros futtatásnál.
- **Skálázhatóság:** Az Amdahl-törvény alapján a gyorsulás felső korlátját a soros rész aránya határozza meg. Mivel ebben a feladatban a programom nagy része párhuzamosítható, magas hatékonyságot tudtam elérni.
