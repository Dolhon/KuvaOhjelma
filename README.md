# KuvaOhjelma
Photo viewing and choosing application

Windows-ohjelmointi IIO11300 Harjoitustyö

Tekijä: Sami Antila

1. Asennus

	- Pura KuvaOhjelma.zip haluamaasi kansioon
	- Käynnistä ohjelma avaamalla KuvaOhjelma.exe tiedosto
	- Järjestelmävaatimukset
		- Windows 7 (32-bit ja 64-bit), Windows 8 (64-bit)
		- 1024MB RAM
		- 20MB tilaa kovalevyllä
		- Microsoft .NET Framework 4.5.


2. Tietoa ohjelmasta (mitä tekee, miksi etc)

	- Toteutetut toiminnalliset vaatimukset
		- Näytä valokuva (*.JPG)
		- Kansion valinta
		- Kansion valokuva tiedostojen listaus
		- Listauksen avulla luo esikatselu kuvat muistiin ja listbox esitys
		- Oikea ja vasen nuolinäppäin navigointi 2 kuvan levyisessä listbox esityksessä
		- Valitsemalla kuvan listasta avaa kuvatiedoston isoon kuva näkymään
		- Koko näytön tila
		- Haluttujen valokuvien valitseminen
		- Kaksoisklikkaamalla isoa kuva näkymää, avaa kuvan kokonäytön ikkunaan
		- Kuvakoon muuttaminen valittuihin kuviin
		- JPG -> PNG konversio valittuihin kuviin
		- Kuvakoon muuttaminen kaikkiin kuviin
		- Lajittelu eri kansioihin ja kansioiden automaattinen nimeäminen
		- Uuden kuvakoon määrittäminen

	- Toteuttamatta jääneet toiminnalliset vaatimukset
		- Config.App tiedosto johonka tallentaa viimeisimmän kansion + valinnat
		- Tiedoston nimen muuttaminen valittuihin/kaikkiin yhtenäiseksi kuva01.jpg, kuva02.jpg...
		- Kuvien arvostelu 0-3
		- Ohjelmassa hiiren 2 valikosta copy kopioi kuvan leikepöydälle
		- Checkbox sisällytettynä listbox ja havainnoillistaa valinnan paremmin
		- Valokuvan zoomaus

	- listaa toiminnallisuus joka toteuttiin ohi/yli alkuperäisten vaatimusten
		- Pikanäppäimet
		- Valikot
		- Toiminnot koko näytön tilassa

	- Ei-toiminnalliset vaatimukset
		- Kuvan vaihto ilman viivettä
		- Kuvan valitseminen yhden painalluksen päässä
		- Mahdollisimman tehokas valokuvien läpi käyminen ja karsiminen
		  (nuolinäppäin + valinta näppäin)

	- Reunaehdot/rajoitukset
		- JPEG formaatti
			- Rajattu kehityksen yksinkertaistamiseksi pelkkään JPEG formaattiin.
			- Ohjelma pystyy avaamaan kaikki yleisimmät kuva formaatit lisäämällä päätteet openfiledialog kohtaan.
		-ListBox
			- SelectedIndex tyhjennys, uutta kansiota avatessa
			- SelectedIndex osoittavien listojen tyhjennys, uutta kansiota avatessa
			- SelectedIndex osoittamaan avattuun kuvaan + reunaehdot listbox kokoon + onko olemassa
		- selectedPhotos list<int>
			- listann tyhjennys, uutta kansiota avatessa
			- Selected/Unselected nappien päivitys, uutta kansiota avatessa
		- Resize/Copy selected/All metodit
			- Tarkistetaan onko valittuja kuvia, jos ei -> hylätään.
		- OpenFileDialog
			- Tarkistus, jos dialogi perutaan -> error
			- Ratkaisu: dialog palautus arvo false -> ohittaa loput metodin toiminnot
		


3. Kuvaruutukaappaukset tärkeimmistä käyttöliittymistä + lyhyet käytöohjeet jollei "ilmiselvää"

	- Pikanäppäimet - Pääikkuna
		- Enter 		– 	Koko näytön tila
		- Esc			– 	Sulje ohjelma
		- Ctrl + O 		– 	Avaa tiedosto
		- E				–	Valitse kuva
		- Vasen nuoli 	– 	Edellinen kuva
		- Oikea nuoli 	– 	Seuraava kuva
		- Alt + F4		-	Sulkee ohjelman

	- Pikanäppäimet - Koko näytön tila
		- Enter 		– 	Sulje koko näytön tila
		- Esc			– 	Sulje koko näytön tila
		- E				–	Valitse kuva (valinta pysyy myös pääikkunassa)
		- Vasen nuoli 	– 	Edellinen kuva (vaihtuu myös pääikkunassa)
		- Oikea nuoli 	– 	Seuraava kuva (vaihtuu myös pääikkunassa)

	- Käyttöliittymä kuva löytyy tästä kansiosta (gui.JPG)


4. Ohjelman tarvitsemat /mukana tulevat tiedostot/tietokannat

	- Ohjelman mukana tulee default.JPG, joka on oletus kuva ohjelman avauksessa (ei välttämätön).
	- Tämä readme.txt tiedosto
	- changelog.txt josta löytyy käytetty aika ja missä järjestyksessä ohjelmaa on kehitetty
	  (ohjelman kansiossa + projektin sisällä)


5. Tiedossa olevat ongelmat ja bugit sekä jatkokehitysideat

	- Ongelmat
		- Muistin käyttö isoissa kuva kokoelmissa, asian ratkaisu olisi streamata esikatselukuvat suoraan kovalevyltä.
		- Ison kuvakokoelman latauksessa käyttöliittymä lakkaa vastaamasta (kotikoneella testattuna kipuraja ~500MB).
		- Valokuvan valinnan parempi havainnoillistaminen (checkbox kuvan kulmaan listbox:ssa).
		- ListBox index out of range (korjattu)
			- Jatkuva riesa alusta loppuun, viimeisenä päivänä 6 testaus kerran jälkeen kaveri ei enään onnistunut 
			  saamaan virhe ilmoitusta.
		- Focus
			- Koko näytön tilassa ensimmäinen nuolinäppäimen painallus asettaa focuksen ja vasta toinen vaihtaa kuvan

	- Jatkokehitysideat 
		- Checkbox listbox:ssa valokuvan kulmassa, havainnoillistaa valitut valokuvat paremmin.
		- Thumbnail säie, ui pysyy toiminnassa esikatselukuvien teon aikana
		- Lataa vain näkyvät valokuvat (ei lataa koko kansion esikatselukuvia kerralla muistiin)
		- Diashow koko näytön tilassa
		- Valokuvan zoomaus
		- Tiedoston nimen muuttaminen valittuihin/kaikkiin yhtenäiseksi kuva01.jpg, kuva02.jpg...
		- Config.App tiedosto johonka tallentaa viimeisimmän kansion + valinnat
		- Kuvien arvostelu 0-3
		- Ohjelmassa hiiren 2 valikosta copy kopioi kuvan leikepöydälle


6. Toteuttamiseen kulunut aika tunteina yhteensä ja tekijöittäin, mitä opittu, mitkä olivat suurimmat haasteet, mitä kannattaisi tutkia/opiskella lisää jne

	- Kulunut aika kaikkiaan: 71 tuntia (tarkemmat tiedot changelog.txt)

	- Opittua
		- WPF käyttöä hyvin monipuolisesti
		- Ikkunoiden välinen kommunikointi
		- Command
		- kuinka tärkeää GUI elementtien nimeäminen on
		- koodin järjestyksen tärkeys
		- Kommenttien tärkeys (pidin 2vk tauon ja sen jälkeen aloin kirjottaa 
		  huomattavasti enemmän kommentteja, kun jouduin tulkitseen koodia)
		- Tiedostojen käsittelyn ongelmia
		- Muistinkäyttö tuli ensimmäistä kertaa vastaan
		- Käytettävyys, pikanäppäimet, käyttäjien intuitio
		
	- Haasteet
		- ListBox, ehdottomasti eniten ongelmia (index out of range)
		- Säikeet, paljon tuli tutkittua, en saanut toimimaan tässä ohjelmassa halutulla tavalla
		- Muistin käyttö

	- Mitä tulisi opiskella lisää
		- Säikeet
		- Data virutalisointia
		- Tiedostojen käsittelyä
			- käytössä olevan tiedoston korvaus
			- tiedoston käyttäminen ja poistaminen samaan aikaan
			- virhetilanteet
		- Kuvankäsittely kirjastot
