---
layout: article
title: Summary
permalink: "/articlessummary.html"
date: 03.03.2013
author: Fabio Salvalai
---

Summary : ({{ paginator.total_pages }})
{% for p in site.pages %}

{% if p.title != page.title %}
<p>
<a href="{{p.url}}">{{p.title}}</a>
</p> 
{% endif %}

{% endfor %}

{% for post in site.posts %}
{{post.title}}: {{post.url}}	
{% endfor %}
