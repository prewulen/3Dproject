# 3Dproject
Projekt realizujący wyświetlanie grafiki 3D w aplikacji WinForms.
![obraz](https://user-images.githubusercontent.com/56601065/119562933-361d2580-bda7-11eb-84a9-c64dad3682a8.png)
![obraz](https://user-images.githubusercontent.com/56601065/119563495-e7bc5680-bda7-11eb-945a-0daaa97ade8d.png)

--Instrukcja obslugi--

A. Wstep

W centralnym polu znajduje sie miejsce animacji.
Po prawej znajduja sie kontrolki opisane nizej.

B. Opis narzedzi po prawej stronie

1. fps - pokazuje liczbe klatek na sekunde
2. Use backface culling - czy podczas animacji ma byc uzywany algorytm obcinania scian tylnych
3. Use z-bufer - czy ma byc uzywany algorytm buforowania glebi
4. Perspective correction - czy ma byc uzywana korekcja perspektywy
5. Draw lines instead of fill - czy zamiast wypelniania trojkatow maja byc rysowane tylko krawedzie
6. Load scene - wczytanie sceny z pliku
7. Save scene - zapisanie sceny do pliku
8. Start - start/stop animacji
9. Lista obiektow - wybor obiektu do edycji/wybor kamery
10. Remove selected item - usuniecie wybranego obiektu (nie mozna usunac kamery 0)
11. Add new item - dodanie nowego obiektu wybranego w comboboxie wyzej
12. Opcje obiektu - dla wybranego obiektu mozna zmieniac wybrane cechy

C. Edycja obiektow oraz przesuwanie kamery

Aby przesunac kamere nalezy przytrzymac srodkowy przycisk myszy i nia poruszac.

Aby obrocic kamere nalezy przytrzymac lewy przycisk myszy i nia poruszac.

Aby przesunac obiekt/swiatlo wzdluz osi x,y,z nalezy przytrzymac klawisz x oraz lewy/prawy/srodkowy przycisk myszy i nia poruszac.

Aby obrocic obiekt wokol osi x,y,z nalezy przytrzymac klawisz c oraz lewy/prawy/srodkowy przycisk myszy i nia poruszac.

Aby przeskalowac obiekt nalezy przytrzymac klawisz v oraz lewy przycisk myszy i nia poruszac.

Aby edytowac cechy obiektu nalezy wybrac go z listy obiektow i pojawia sie opcje u dolu prawej strony.
