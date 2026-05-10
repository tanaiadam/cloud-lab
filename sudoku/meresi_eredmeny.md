# Mérési Dokumentáció: Sudoku Megoldó Párhuzamosítása

## 1. Célkitűzés
A feladatom célja egy hatékony párhuzamos Sudoku megoldó készítése volt a Para klaszteren. Olyan algoritmust választottam, amely képes a teljes keresési fa bejárására az összes lehetséges megoldás megtalálása érdekében.

## 2. Módszertan
- **Párhuzamosítás:** A visszalépéses (backtracking) algoritmust OpenMP Taskok (`#pragma omp task`) használatával párhuzamosítottam a keresési fa bejárásához.
- **Granularitás szabályozása:** A túl sok szál létrehozásának elkerülése és az adminisztrációs többletköltség csökkentése érdekében bevezettem egy mélységi korlátot. A szálakat csak a keresési fa legfelső szintjein hoztam létre.
- **Szinkronizáció:** A megoldások számolásához atomi műveleteket (`#pragma omp atomic`) és szinkronizációs pontokat (`#pragma omp taskwait`) alkalmaztam.
- **Bemenet:** Az `easy.sdk` fájlban található 50 darab 9x9-es tábla összesített feldolgozását végeztem el.

## 3. Mérési Eredmények
A méréseim az 50 tábla együttes futási idejét (Total elapsed time) mutatják 1, 2, 3 és 4 magon.

| Magok száma | Futási idő [s] | Gyorsulás | Hatékonyság |
|:---:|:---:|:---:|:---:|
| 1 | 0.266084 | 1.00 | 100.0% |
| 2 | 0.176078 | 1.51 | 75.5% |
| 3 | 0.174189 | 1.53 | 51.0% |
| 4 | 0.101366 | 2.62 | 65.5% |

## 4. Elemzés és Interpretáció
- **Szublineáris gyorsulás:** Azt tapasztaltam, hogy a program gyorsul, de a görbe elmarad az ideálistól. Megállapítottam, hogy ennek legfőbb oka az adminisztrációs többletköltség (overhead).
- **Granularitás hatása:** Megfigyeltem, hogy mivel a bemenetként kapott táblák rendkívül könnyűek, az OpenMP taskok létrehozása és ütemezése jelentős részt vesz el a teljes futásidőből a tényleges számításhoz képest. 
- **Skálázhatóság:** Úgy vélem, nehezebb táblák esetén a keresési fa mélyebb lenne, így a párhuzamosítás hatékonyabban működne. 4 mag esetén a 2.62-szeres gyorsulás jelzi számomra, hogy az algoritmusom képes kihasználni a párhuzamos erőforrásokat a felügyeleti költségek ellenére is.
