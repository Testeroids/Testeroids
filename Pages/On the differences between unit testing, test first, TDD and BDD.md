---
layout: article
title: A note on the differences between unit testing, test first, TDD and BDD
date: 03.05.2013
author: Fabio Salvalai
published: false
---
As a manager, or as a newcomer to the art of unit testing, you will most of the time hear people around you talking about "writing tests" or "unit-testing" without any distinction regarding '''how''' they actually proceed. There are 4 major approaches into writing unit tests, and a world of difference between them.


##What does Unit test mean anyway ?
Before going further into the differences between those approaches, we first need to make sure we're on the same page. You might already have a strong sense regarding what a unit test is, but I'll try to be concise and maybe add some meaningful nuances to your conception of unit tests.

###Common misconception
The word Unit-test itelf has quite a lot of meaning. Most people only consider the "test" word, and overlook the meaning of what "unit" means.
To many people, *unit*-testing means: take your big monolithic software, carve out a reasonably-sized slice out of it, consider it as a black box, and try to insufflate life into this detached part of software. Then, you will stimulate this black box and assert that what comes out it is what you expect to.

Well, guess what? You aren't testing a unit at all. Usually, when doing things this way, what happens is that the size of the black box is simply too big and too complex, because of all the dependencies it requires. In fact, you would be basically stripping the UI off of your software, and programmatically checking the values returned by your black box, instead of checking the same values on-screen with the usual GUI.

This practice can be useful, but those are called *System tests*, or maybe *Integration tests*, but not unit tests.

###The way it is supposed to be.
Now, if you want to do it right, and write real *unit* tests, don't think about it as "testing a slice of a monolithic software". Think in terms of "making sure the atoms which compose it are '''all''' doing what they are supposed to do, *one atom at a time*."

Insufflating life into a big slice of software is pretty easy. usually, you would identify a class on the top of the food chain, one without parameters in the constructor, or with parameters you can easily forge with '''stubs''', and off you go. Problem is : since your subject under test is the pinacle of the food chain, all its dependencies are live objects. That means that if your subject under test is supposed to make a file system access, a network connection, interactions with hardware devices, or waiting for an event to happen, you will have to do it all in a test environment also, and spend a great deal of effort into ceremony code in order to forge a situation which reflects the context in which you want to test your subject under test. Worse event, your tests might have to change the configuration of the machine on which you are running your tests, for instance if you need to write a file on disk if it doesn't exist, it would work the first time you run the tests, but next time you run it, the context would have changed, resulting in flaky tests. Of course, you can always clean-up after you finish your test, but there are still two problems with that : the first, you will never be able to run tests in parallel and leverage multi-core CPUs. The second is that the problem with system-intrusive tests, you never know if the state induced by a test was because it was the purpose of the test, or if the system was already in that state before. You could the hosting machine in a known state, of course, but you probably found it this way or a reason. It seems rather obvious that you should always leave the hosting system as you found it. you would then have to keep tabs on the state things were before you did your thing, track the changes made to your system by the subject under test, and revert everything. You certainly don't want to do that, and that would imply you have to know all the internals of your subject under test, and what might be true one day could be refactored the day after. You definitely don't want to keep maintaining this kind of things.

###Dependency injection and Mocks.
The answer to this problem is quite simple : Use mocks and dependency injection.

####mocks
If your subject under test must write a file to the disk : mock the file system away.
Mocks are not to be confused with stubs. Stubs are implementations written by hand by the developer to return specific values. 
Mocks are configurable, they usually come as libraries or frameworks. My own personal favorite one for the .Net language is moq, therefore, this is what the Testeroids framework uses it under the hood, but there are many others. They all fit the same purpose, just pick the one whose syntax suits you most.
In a nutshell, a mock will allow you to create an object which will implement the desired interface. the difference between stubs and mocks is that you don't need to write the class for it. The mocking framework will provide you wuth a way to define the behavior of your mocked object, and to kep tabs on which methods have been called, which properties have been accessed, how many times, with which parameters, and so on. You can really think of a mocked object as a monitorable zombie object which will only reply what you taught him to reply on specific conditions, stripping it of any implementation you might expect from a real-life object.

This is, by the way, why you should always respect Liskov's substitution principle and always, I mean *always* code against contracts instead of concrete classes, even though there will only ever be one single implementation for this contract. If it sounds like a big overhead of work to your ear, that just means you are not using the right tools to write your code. Have a look into ReSharper if you are a .Net developer. It's a paid software for commercial use, but it deserves every penny.

####dependency injection
Now, coding against interfaces will not help you much if you new-up every object your subject under test depends on, because since the decision of new-ing it up is taken in the code of the method using it, the tests can never replace this object with a mock. This is where dependency injection enters the equation.
Every time you need to rely on a service, inject it. Don't use a static method, don't use a service locator, inject it.
Dependency injection is no rocket science, it's just a fancy word to say that you are passing the service you require as a constructor argument. This alone will let you having your tests inject a mocked version of your object, which you can keep tabs on, and configure to behave the way you want him to.

##The 4 major approaches

###"Code First" unit-testing
The most naive approach about unit testing. Usually the first one you will ever put in place in your carreer. The problem with this aproach is that it gives you absolutely no added value regarding emerging architecture or requirements specification. Basically, you are going to write your software without having testability in mind. Lots of classes will be tightly coupled, and since you didn't have to try and make an effort into mocking your dependencies, separation of concerns will probably not be your priority. Chances are, it is going to become so expensive to test your code after writing it, that you will simply give up under the pressure of a deadline. Code-first testing will never ever help you write your code ant it will obviously never speed-up the development, since all the time spent writing tests will take place after the code has been wrapped-up. It still has as least the merits of warning you in case of regressions. Some tools, like Pex, will help you generating a complete test suite which will do exactly what you don't want a unit test to do : It will write green tests, whether the logic of your code is good or flawed. Those are nice and cheap anti-regression tools, but that's all there is to it.
###"Test First" unit-testing
The Red-Green factor ! The whole philosophy of test-first is about writing a test which will first fail, and for which you'll have to write the code to make it pass.
First, wrte the simplest, dumbest code to make the test go from red to green. If for a calculator, you write a test which tests that the addition of 3 plus 2 should equal 5, then hardcode 5 as the return value of your calculator. That's it, your job is done. If the application is obviously not doing what it should, then you might just have forgotten a test. Play dumb, really. It's not easy, because hey, we are supposed to be smart, right ? The key to succeeding in test-first is to be really creative about how dumb you can get. if you need to return an instantiated class, just return new object(); do everything you can to be as anti-goal oriented in your code, and then, switch back to your tests, and be very demanding about what you want your tested method should return. If your code has been sucessful into making your tests go green, even if the code is absurd, don't delete that test. It's not that he's not complete enough, it's just that there's another missing : Practice '''triangulation''' : try to test what the result of your Add() method is if you pass, not 3 plus 2, but 6 plus 7. if you hardcoded 5, then it will fail. If you hardcode it to 13, then the first test you wrote will fail. Now, you have no other choice than doing the real deal and have a proper addition. If you can't find any way to play dumb any longer, then run your application and test it with real cases : you will most likely have an application that behaves the way you want to.
###Test Driven Development (TDD)
While many people which are sensibilized to the question of unit testing will agree to practice test-first, but most of them will save this practice for difficult area in the business logic, leaving the "easy" parts out.
###Behavior Driven Development (BDD)
