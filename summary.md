---
layout: article
title: Summary
permalink: "/articlessummary.html"
---

# Summary : ({{ paginator.total_pages }})
{% for page in site.pages %}
		{{page.title}}: {{page.url}}	
{% endfor %}

{% for post in site.posts %}
		{{post.title}}: {{post.url}}	
{% endfor %}
