---
layout: article
title: Summary
permalink: "/articlessummary.html"
date: 03.03.2013
author: Fabio Salvalai
---

Summary : ({{ paginator.total_pages }})
{% for page in site.pages %}
<!-- link -->
<p>
<a href="{{page.url}}">{{page.title}}</a>
</p> 
{% endfor %}

{% for post in site.posts %}
{{post.title}}: {{post.url}}	
{% endfor %}
