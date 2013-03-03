---
layout: article
title: Getting started with Testeroids
permalink: "/articlessummary.html"
date: 03.03.2013
author: Fabio Salvalai
---
{% for p in site.pages %}

{% if p.title != page.title %}
<p>
<a href="{{site.baseurl}}{{p.url}}">{{p.title}}</a>
</p> 
{% endif %}

{% endfor %}

