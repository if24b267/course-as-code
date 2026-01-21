# CollatzSequence

## Synopsis

Berechnen Sie alle Werte in der Collatzfolge!

## Einleitung

[^collatzseq-wiki-ref]: [Collatz-Problem (Wikipedia)](https://de.wikipedia.org/wiki/Collatz-Problem)

[^collatzseq-veritasium-ref]: [The Simplest Math Problem No One Can Solve - Collatz Conjecture (Veritasium)](https://youtu.be/094y1Z2wpJg)

Das Collatz-Problem ist ein ungelöstes mathematisches Problem[^collatzseq-wiki-ref] und "the most dangerous problem in mathematics"[^collatzseq-veritasium-ref]. Es handelt von der Zahlenfolge

$x_{n+1}=
\begin{cases}
x_n/2     &\text{if $x_n$ even}\\
3 x_n + 1 &\text{if $x_n$ odd}
\end{cases}$

und stellt die Frage ob die Zahlenfolge für jede beliebige Eingabe zur Zahl 1 gelangt.

In diesem Beispiel sollen sie alle Elemente der Reihe bis zur Zahl 1 berechnen.

## Aufgabe

Lesen Sie eine ganze Zahl ein und geben Sie alle Nachfolger in der Collatz Folge aus. Sobald die Zahl 1 erreicht wurde stoppt das Programm.

- Eingabeaufforderung: `": "`
- Das Programm endet mit eine leeren Zeile

## Beispiele

## Standardfall

(Text in rot = Benutzereingabe)

> ~~~~{.console}
> : \stdin{1}
> 1
> ~~~~~

> ~~~~{.console}
> : \stdin{100}
> 100
> 50
> 25
> 76
> 38
> 19
> 58
> 29
> 88
> 44
> 22
> 11
> 34
> 17
> 52
> 26
> 13
> 40
> 20
> 10
> 5
> 16
> 8
> 4
> 2
> 1
> ~~~~~