---
layout: article
title: A note on how to make your unit tests more unit-like
date: 03.05.2013
author: Fabio Salvalai
published: false
---

We [Previously](On%20the%20differences%20between%20unit%20testing,%20test%20first,%20TDD%20and%20BDD.html) established that when writing unit test, rather than testing a part of a monolithic application, where the part could get fairly big, it was preferable to minimize the tested surface and have *atomic* tests. But we didn't get into much details regarding *how* to do it.

#Doing it right
Insufflating life into a big slice of software is pretty easy. Usually, you would identify a class on the top of the food chain, one without any parameter in the constructor, or with parameters you can easily forge with *stubs*, and off you go. Problem is : since your *subject under test* is the pinnacle of the food chain, all its dependencies are real objects, with concrete implementation for their methods. That means that if your subject under test is supposed to make a file system access, a network connection, have interactions with hardware devices, or wait for an event to happen, you will have to do it in your test environment as well. This also means you will have to spend a great deal of effort into *ceremony code* to put your *subject under test* in the state in which you want to test its methods.

##Don't be intrusive
Worse even, your tests might have to change the configuration of the machine on which you are running them. For instance if you need to write a file on disk only if it doesn't exist. It would work the first time you run the tests, but next time you run it, the context of the host machine would have changed, resulting in flaky tests. Of course, you could always argue that you can clean-up after you finish your test, but there are still two problems with that. First, you will never be able to run multiple tests in parallel and leverage multi-core CPUs. The second is that, with system-intrusive tests, you never know if the state of your system at the end of a test was so because it was the purpose of the test, or if it was already in that state before. Of course, you could reset the hosting machine in a known state before running your tests, but the hosting machine was probably in that state for a reason. It seems rather obvious that you should always leave the hosting machine as you found it. If your tests are intrusive, you would then have to keep tabs on the state things were before you ran your tests, track the changes made to your system by the *subject under test*, and revert everything.You certainly don't want to do that, as it would imply you have to know all the internals of your *subject under test*. Keep in mind that what might be true one day could be refactored the day after. You definitely don't want to keep maintaining this kind of things.

###Dependency injection and Mocks.
The answer to this problem is quite simple: Use mocks and dependency injection.

####mocks
If your *subject under test* must write a file to the disk: mock the file system away.

Mocks are not to be confused with stubs. Stubs are hand-rolled implementations hard-coded by the developer to return specific values.
 
Mocks are configurable, they usually come as libraries or frameworks. My own personal favorite for the .Net language is [*moq*](http:// "http://github.com/moq"), therefore, this is what the Testeroids framework uses it under the hood, but there are many others. They all fit the same purpose, just pick the one whose syntax suits you most.
In a nutshell, a mock will allow you to create an object which will implement a given interface. the difference between stubs and mocks is that you don't need to write the class for it. The mocking framework will provide you with a way to define the behavior for each method in an interface, and make an instantiated object out of it. It will keep tabs on which methods have been called, which properties have been accessed, how many times, with which parameters, and so on. You can really think of a mocked object as a monitorable zombie object which will only reply what you taught him to reply on specific conditions, stripping it of any implementation you might expect from a real-life object. If your zombie object is being used in unknown conditions (i.e. a method call for which no configuration has been applied beforehand) the mocked object will either throw an exception or return null, depending on your mocking framework and how you configured it.

This is, by the way, why you should always respect Liskov's substitution principle and always, I mean *always* code against contracts instead of concrete classes, even though there will only ever be one single implementation for this contract. If you don't code against contracts you will not be able to use mocking correctly. You could always argue that some products like *Microsoft's Fakes*, *TypeMock* or Telerik's *JustMock pro* can get the job done, but you'll end up paying the price for both the licenses and the performances, as those advanced frameworks use very heavy profiler API's and will severely impact the performance of your tests. If implementing an interface for each and every class in your program sounds like a big overhead of work to your ear, that just means you are not using the right tools to write your code. Have a look into [ReSharper](http://www.resharper.com) if you are a .Net developer. It's free for open-source projects, and paid software for commercial use, but it deserves every penny.

####dependency injection
Now, coding against interfaces will not help you much if you new-up every object your *subject under test* depends on. The decision of new-ing up an object of a specific implementation is taken in the code of the method being tested. Therefore, the tests can never replace this object with a mock. This is where dependency injection enters the equation.
Every time you need to rely on a service, inject it. Don't use a static method, don't use a service locator, inject it!
Dependency injection is no rocket science, it's just a fancy word to say that you are passing the service you require as an argument to the constructor. This alone allow your tests to inject a mocked version of your object. From then on, you can keep tabs on it and configure it to behave the way you want it to.