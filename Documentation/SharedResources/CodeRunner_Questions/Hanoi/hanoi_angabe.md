# Hanoi

## Synopsis

Lösen Sie die *Türme von Hanoi* rekursiv.

## Einleitung

Die *Türme von Hanoi* ist ein Knobelspiel.
Es besteht aus drei *Stäben* ($A$, $B$, $C$) und $n$ *Scheiben*. Alle Scheiben haben unterschiedliche *Größe*. Die Größen reichen von 1 (der kleinsten Scheibe) bis $n$ (der größten Scheibe). Zu Beginn des Spiels sind alle Scheiben der Größe nach auf einem Stab gestapelt - die größte Scheibe zu unterst, die kleinste zu oberst (siehe Abb. \ref{fig:hanoi}).

![Türme von Hanoi mit 8 Scheiben in Anfangsposition\label{fig:hanoi}](hanoi.jpg)

Ziel des Spiels ist es alle Scheiben auf einen anderen Stab umzuschichten. Dabei gelten folgende Regeln

- in einem *Zug* darf nur eine **einzige** Scheibe bewegt werden
- von einem Stapel darf nur die **oberste** Scheibe entfernt werden
- eine Scheibe darf nur auf eine **größere** gelegt werden (oder einen leeren Stab)

Man kann zeigen, dass das Spiel für jede beliebige Anzahl $n$ von Scheiben in endlich vielen Schritten lösbar ist. Die Anzahl der Züge ist $2^n-1$.

Die Lösung lässt sich sehr elegant rekursiv ausdrücken (siehe Funktion `move`{.java} weiter unten).

Unten angeführte Funktion produziert für 3 Scheiben beispielsweise folgene Sequenz von Zügen:

> ~~~{.console}
> --> Moving disk 1 from rod A to rod C
> --> Moving disk 2 from rod A to rod B
> --> Moving disk 1 from rod C to rod B
> --> Moving disk 3 from rod A to rod C
> --> Moving disk 1 from rod B to rod A
> --> Moving disk 2 from rod B to rod C
> --> Moving disk 1 from rod A to rod C
> ~~~

## Aufgabe

- Studieren Sie die angegebene Lösung `move`{.java} am besten mit dem Debugger
- Ausgehend von `move`{.java}, erstellen Sie eine Funktion `towersOfHanoi`{.java}, welche
  - die *Anzahl* der ausgeführten Züge zurückgibt
  - Ausgabe produziert, die rekursionstiefenabhängig eingerückt ist (siehe Beispiel)

Pro Rekursionsebene wird um zwei Leerzeichen eingrückt (die bereits in `move`{.java} vorhandene Ausgabe können Sie verwenden). Verwenden sie zum Testen die angegebene `main`{.java} Funktion.

## Beispiele

(Text in rot = Benutzereingabe)

> ~~~{.console}
> discs: \stdin{3}
> 
> towersOfHanoi(3, A, C) |
>   towersOfHanoi(2, A, B) |
>     towersOfHanoi(1, A, C) |
>       --> Moving disk 1 from rod A to rod C
>     |
>     --> Moving disk 2 from rod A to rod B
>     towersOfHanoi(1, C, B) |
>       --> Moving disk 1 from rod C to rod B
>     |
>   |
>   --> Moving disk 3 from rod A to rod C
>   towersOfHanoi(2, B, C) |
>     towersOfHanoi(1, B, A) |
>       --> Moving disk 1 from rod B to rod A
>     |
>     --> Moving disk 2 from rod B to rod C
>     towersOfHanoi(1, A, C) |
>       --> Moving disk 1 from rod A to rod C
>     |
>   |
> |
> 7 move(s) in total.
> ~~~

## Hinweis

### `main`{.java}

~~~{.java .numberLines}
    public static void main(String args[]) {
        Scanner sc = new Scanner(System.in);
        System.out.printf("discs: ");
        int n = sc.nextInt(); 
        System.out.println();
        System.out.printf("%d move(s) in total.\n", towersOfHanoi(n, "A", "C", "B"));
        sc.close();
    }
~~~

### `move`{.java}

~~~{.java .numberLines}
/**
     * Recursive function to solve the towers of hanoi problem.<br>
     * 
     * This function produces textual output of the necessary moves to solve the
     * tower of hanoi problem for any number of discs.<br>
     * It moves a disc of specified size n from a specified from_rod to another
     * specified to_rod (and uses another specified temp_rod as temporary storage).
     * It does this by
     * 
     * <ul>
     * <li>(recursively) moving all smaller discs (size&lt;n) to the temporary
     * rod</li>
     * <li>moving disc of size n to to_rod</li>
     * <li>(recursively) moving all smaller discs (size&lt;n) from the temporary rod
     * to the to_rod</li>
     * </ul>
     * 
     * 
     * @param n
     *            the size of the disk we want to move
     * @param from_rod
     *            the rod where the disk is before the move
     * @param to_rod
     *            the rod where the disk should be moved to
     * @param temp_rod
     *            the rod where other disks than the one we are moving can be
     *            temporarily stored
     */
    static void move(int n, String from_rod, String to_rod, String temp_rod) {

        if (n == 1) {
            System.out.println("--> Moving disk 1 from rod " + from_rod + " to rod " + to_rod);
            return;
        }
        move(n - 1, from_rod, temp_rod, to_rod);
        System.out.println("--> Moving disk " + n + " from rod " + from_rod + " to rod " + to_rod);
        move(n - 1, temp_rod, to_rod, from_rod);

    }

    public static void demo() {
        Scanner sc = new Scanner(System.in);
        int n = sc.nextInt(); // Number of disks
        move(n, "A", "C", "B"); // A, B and C are names of rods
        sc.close();
    }

~~~