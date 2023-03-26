# Web app solution

1. In this folder, I turned the stream parser into a web service based on my knowledge, so the parser can be run in validation pipeline or in scenarios
when users upload stream files from web
2. This solution contains mixture of running program and pseudocode. Unit tests are runnable, on the other hand, web app configurations such as Inversion of Control and the actual HTML client-facing interface are pseudocode. 
3. I used .Net MVC as the web app backbone. 
4. I created an application service, where the stream validator lived in. Because the parser now is an application service, so it can be plugged into different controller actions or other services using dependency injection. 
5. The validator service has a method that contains logic for the parser. The method validates input stream based on the requirements, and returning a response to indicate if stream is valid, and also containing information of a success message and an error message. 
5. Unit tests written using xUnit cover logic fo the validtor service.
6. This project is mainly to demonstrate my ideas around turning the parser into a service that can be used in different places in an application, including unit testings, project structure, and coding styles. 
