# Age_of_Empires

## Synopsis

Wiederholen sie Ausgabeformate für ganze Zahlen!

## Assignment

Schreiben Sie ein Java-Programm, das nach der Eingabe Ihres Alters dieses unterschiedlich formatiert wieder ausgibt.

Folgende Formate sollen angewandt werden

Format | Hinweis
:- | :-
%d | Zahl
%+d | Zahl mit Vorzeichen
% d | Zahl mit Platzhalter für Voreichen
%5d | Zahl, 5 Stellen
%05d | Zahl, 5 Stellen & führende 0
%-5d | Zahl, 5 Stellen linksbündig
%#x | Zahl, Hexadezimal

## Beispiel

(Text in rot = Benutzereingabe)

~~~{.console}
Your age: \stdin{37}
%d:   ~37~
%+d:  ~+37~
% d:  ~ 37~
%5d:  ~   37~
%05d: ~00037~
%-5d: ~37   ~
%#x:  ~0x25~
~~~

## Hinweise

* Vergessen Sie bitte nicht das `\n`{.java} am Ende von `System.out.printf()`{.java} Anweisungen! 
* Die `~`{.java} dient der "Begrenzung" der Ausgabe.