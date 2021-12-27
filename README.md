# .NET Microservice Örnek Rehber Uygulaması

Mikroservis mimarisi ile oluşturulmuş rehber uygulaması.

## Başlarken

Docker uygulamasının kurulu ve konfigürasyonu yapıldığına eminseniz , aşağıdaki kodu çalıştırarak `Phonebook Applicaton` uygulamasını hazır hale getirebilirsiz. 

```powershell
docker-compose up --build
```

## Seneryo

Birden fazla mikroservis kullanarak basit bir rehber uygulaması geliştirilmesi.

- Rehberde kişi oluşturma
- Rehberde kişi kaldırma
- Rehberdeki kişiye iletişim bilgisi ekleme
- Rehberdeki kişiden iletişim bilgisi kaldırma
- Rehberdeki kişilerin listelenmesi
- Rehberdeki bir kişiyle ilgili iletişim bilgilerinin de yer aldığı detay bilgilerin getirilmesi
- Rehberdeki kişilerin bulundukları konuma göre istatistiklerini çıkartan bir rapor talebi
- Sistemin oluşturduğu raporların listelenmesi
- Sistemin oluşturduğu bir raporun detay bilgilerinin getirilmesi



## Docker Containers

| Image   | Port  | Host   | 
|---|---|---|
|  phonebook/gateway |  5000 | localhost  |
|  phonebook/phonebook | 5010  | localhost  |
|  phonebook/report | 5020  | localhost  |
|  phonebook/datacapture | 5201  | localhost  |
|  rabbitmq | 15672  | localhost  |


## Yapının Özeti 


Yapı 4 mikro servis , 2 veritabanı ve 1 mesaj dinleyiciden oluşmaktadır. 

1. İlk mikro servisimiz **Gateway** 'dir . Yük dengelenmesi, kimlik doğrulama ya da yetkilendirme , diğer bütün mikroservisleri ortak bir yerden yönetimi için kullanılabilir. 

2.  Kişilerin rehbere eklenmesi , silinmesi , güncellenmesi vb. işlemleri yapan mikroservisimiz **Phonebook** dir.

3.  Raporların listelenmesi , raporların detay bilgilerinin getirilmesi , içerideki veriler doğrultusunda raporların hazırlamakla yükümlü olan   mikroservisimiz **Report** dir.

4.  Raporlama servisi için **Phonebook** mikro servisinin gönderdiği mesajları dinleyip , ilgili kayıtları rapor servisinin veritabanına yazan servisimiz **DataCapture** dir.
    

Yapının genel şeması aşağıdaki gibidir.
![schema](https://user-images.githubusercontent.com/88135079/147427722-061c320f-8e99-4d27-aba5-66ef6c5a6dcd.PNG)


## Teknoloji ve Eklentiler 

- .Net 5
- RabbitMQ
- MassTransit
- Event Source
- CQRS && DDD
- Docker
- Ocelot
- PostgreSQL
- MongoDB
- Entity Framework Core
- Entity Framework Core In-Memory DB
- Portainer
- Fluent Validation
- AutoMapper
- Fluent Assertions
- Moq

# Ekran Görünütüleri 
![Capture1](https://user-images.githubusercontent.com/88135079/147427777-859df46e-8464-403d-9c97-e727bea52340.PNG)


## RabbitMQ
![Capture2](https://user-images.githubusercontent.com/88135079/147427783-a8360838-2f54-451b-bd3b-27872ad8ebf7.PNG)


## Postgres
![Capture3](https://user-images.githubusercontent.com/88135079/147427791-0d52619c-4896-4928-90a5-14909d8424af.PNG)


## Covarage
![Capture4](https://user-images.githubusercontent.com/88135079/147427792-f4f83d80-5919-4b8b-9c68-4f79b5f1a2a3.PNG)



