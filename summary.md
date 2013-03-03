---
layout: article
title: Summary
permalink: "/articlessummary.html"
date: site.time
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
