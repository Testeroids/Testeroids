---
layout: article
title: Summary
permalink: "/articlessummary.html"
---

# Summary : ({{ paginator.total_pages }})
{% for page in site.pages %}
	{% if forloop.index < 10 %}
		<a href="{{page.url}}">{{page.title}}</a>
	{% endif %}
{% endfor %}
