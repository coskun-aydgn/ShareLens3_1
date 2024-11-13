CRUD
  Create: burada kullanicidan gerekli bilgileri alip yeni bir post olusturup, tabloda eklemektir. bunu yaparkenilk denememizde kullanicidan baslik, tanim doldurmasi ve resmi upload 
  etmesini istedik.bunun icin kullanici bu bilgileri girdikten sonra create tusu yardimiyla submit edince PostController de create metodu tetiklenecek. 
  bu metodu olustururken  ilk problemim tarihi DateTime.Now metodunu kullandim. ama benim entity imde string olarak kaydedilmisti ve saat kullanmayacaktim. bundan dolayi hata 
  aldim. bunu duzeltmek icin DateTime.Now.ToString("dd.MM.yyyy") sekilde duzeltme yaptim. daha sonra yine bu entity de olsturdugum "public string CreatedDate { get; set; }" yaptigimiz icin
  degerler submit yapilinca CreatedDate required hatasi aldim. cunku kullanicidan almam greken bilgiler arasinda olsuturma tarihi gerekli oldugu icin hata aldik. bunun cozumunu 
  CreatedDate variablinin null olabilir sekilde duzenleyince ("public string? CreatedDate { get; set; }") problemimiz artadan kalkti.sonraki problemimiz kullanicin yukedigi resmin
  boyutuna gore goruntu bozuluyordu.
  Updete: update i ayarlarken en cok zorlandigimz nokta resimi degistirmek oldu. cunku bir turlu yeni resim icin Url olsuturmak icin problem yasadik. hatta form olsutururken " enctype="multipart/form-data"" ifadesini yazmayi unuttugumuz icin problem yasadik. 3. bir goz bakinca problemimiz cozuldu. ayrica resime tiklandiginda yeni resim yuklemek icin basit bir js fonksiyonu kullandim.
Asynchronous database access: control dosyasindaki metodlari async yapildi. ayriva Lazy loading yapildi. bu arada database deki entity ler arasi bag kuruldu.
Authentication: onceden User models olsuturdugumdan authentication yapinca IdentityUser ile cakismalar olde ve sistem calismadi. bununla ilgili duzenlemler yapip bunu repository, model, ve controlleri gozden gecirmek zorunda kaldik.
