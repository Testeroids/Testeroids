---
layout: article
title: Summary
permalink: "/articlessummary.html"
---

# Summary : ({{ paginator.total_pages }})
{% for page in site.pages %}
		{{page.title}}: {{page.url}}	
{% endfor %}
