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
If your subject under test must write a file to the disk : mock the file system away.
Mocks are not to be confused with stubs. Stubs are implementations written by hand by the developer to return specific values. 
Mocks are configurable, they usually come as libraries or frameworks. My own personal favorite one for the .Net language is moq, therefore, this is what the Testeroids framework uses it under the hood, but there are many others. They all fit the same purpose, just pick the one whose syntax suits you most.
In a nutshell, a mock will allow you to create an object which will implement the desired interface. the difference between stubs and mocks is that you don't need to write the class for it. The mocking framework will provide you wuth a way to define the behavior of your mocked object, and to kep tabs on which methods have been called, which properties have been accessed, how many times, with which parameters, and so on. You can really think of a mocked object as a monitorable zombie object which will only reply what you taught him to reply on specific conditions, stripping it of any implementation you might expect from a real-life object.

This is, by the way, why you should always respect Liskov's substitution principle and always, I mean *always* code against contracts instead of concrete classes, even though there will only ever be one single implementation for this contract. If it sounds like a big overhead of work to your ear, that just means you are not using the right tools to write your code. Have a look into ReSharper if you are a .Net developer. It's a paid software for commercial use, but it deserves every penny.

Now, coding against interface will not help you much if you new-up every object your subject under test depends on. [ToBeContinued]

##The 4 major approaches

###"Code First" unit-testing
###"Test First" unit-testing
###Test Driven Development (TDD)
###Behavior Driven Development (BDD)
