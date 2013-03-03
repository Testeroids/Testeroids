---
layout: article
title: Articles summary
permalink: "/articlessummary.html"
---

very nice

# Summary : ({{ paginator.total_pages }})
{% for article in site.pages %}
	{% if forloop.index < 10 %}
		<a href="http://www.google.com">article.title</a>
	{% endif %}
{% endfor %}
