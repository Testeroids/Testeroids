---
layout: article
title: A note on the differences between unit testing, test first, TDD and BDD
date: 03.05.2013
author: Fabio Salvalai
published: false
---
As a manager, or as a newcomer to the art of unit testing, you will most of the time hear people around you talking about "writing tests" or "unit-testing" without any distinction regarding '''how''' they actually proceed. There are 4 major approaches into writing unit tests, and a world of difference between them.


##What does Unit test mean anyway ?
Before going further into the differences between those approaches, we first need to make sure everybody is on the same page. You might already have a strong sense regarding what a unit test is, but I'll try to be concise and maybe add some meaningful nuances to your conception of unit tests.

###Common misconception
The word Unit-test itelf has quite a lot of meaning. most people only considere the "Test" word, and overlook the meaning of what "unit" means.
To many people, *unit*-testing means : take your big monolithic software, take a reasonably-sized slice out of it, consider it as a black box, and try to insufflate life into this detached part of software. then, you will stimulate this black box and assert that what comes out it is what you expect to.

Well, guess what ? you aren't testing a unit at all. Usually, when doing so, what happens is that the size of the black box is too big, because of all the dependencies it requires. In fact, you would be basically stripping the UI off your software, and programmatically check the values returned by your black box, instead of checking the same values on-screen with the usual GUI.

This practice can be useful, but those are called *System tests*, or maybe *Integration tests*, but not unit tests.

###The way it is supposed to be.
Now, if you want to do it right, and write real *unit* tests, don't think about it as "testing a slice of a monolithic software". Think in terms of "making sure the atoms which compose it are '''all''' doing what they are supposed to do, *one atom at a time*."

Insuflating life into a big slice of software is pretty easy. usually, you would identify a 

##The 4 major approaches

###"Code First" unit-testing
###"Test First" unit-testing
###Test Driven Development (TDD)
###Behavior Driven Development (BDD)
