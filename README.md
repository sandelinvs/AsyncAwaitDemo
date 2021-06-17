# AsyncAwaitDemo

Ska labba med async/await, nunit, ef och paralell extensions

uppgift:
1. scrapa sfbokhandelns hemsida för att plocka ut boktitlar (namn, författare, bild)
2. ladda hem alla bilder paralellt och spara till disk med async streams
3. konstruera en egen htmlfil med det scrapade innehållet
4. spara det scrapade datat i dbn mappad med EF (async)
5. sätt upp nunit så att det går att köra tester med en inmemory databas samt den riktiga dbn utan att behöva ändra testcase kod.
  5.ska vara möjligt att stubba bort webben och tjonka in en förladdad sida från en dummy/stubbe
