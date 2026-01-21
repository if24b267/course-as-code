# Hyperbel

## Synopis

Geben sie eine beschriftete Hyperbel aus!

## Assignment

Schreiben Sie ein Programm, das vier ganze Zahlen a, b, c und d sowie eine Fließkommazahl e
einliest und genau die angegebene Ausgabe erzeugt. Verwenden Sie zum Ausrichten
ausschließlich Leerzeichen. Die Zahlen a, b, c und d werden oben, unten, links und rechts
ausgegeben. b ist linksbündig, c rechtsbündig formatiert, Spaltenbreite jeweils 3. a und d sind
rechtsbündig, die Spaltenbreite beträgt 11 Stellen. e ist ebenfalls rechtsbündig mit einer
Spaltenbreite von 7 Stellen.

## Beispiele

(Text in rot = Benutzereingabe)

### Beispiel 1

> ~~~~{.console}
> a: \stdin{1}
> b: \stdin{2}
> c: \stdin{3}
> d: \stdin{4}
> e: \stdin{5.5}
>
> 01234567890123456
> \textbackslash               /
>  \textbackslash 00000000001 /
>   \textbackslash           /
> 2  | 005.500 |  3
>   /           \textbackslash
>  / 00000000004 \textbackslash
> /               \textbackslash
> 
> ~~~~~

### Beispiel 2

> ~~~~{.console}
> a: \stdin{111}
> b: \stdin{222}
> c: \stdin{333}
> d: \stdin{444}
> e: \stdin{12.23}
>
> 01234567890123456
> \textbackslash               /
>  \textbackslash 00000000111 /
>   \textbackslash           /
> 222| 012.230 |333
>   /           \textbackslash
>  / 00000000444 \textbackslash
> /               \textbackslash
> 
> ~~~~~