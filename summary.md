---
layout: article
title: Summary
permalink: "/articlessummary.html"
---

# Summary : ({{ paginator.total_pages }})
{% for page in site.pages %}
<!-- link -->
{% raw %}
<a href="{{page.url}}">{{page.title}}</a><br/>
{% endraw %}
{% endfor %}

{% for post in site.posts %}
{{post.title}}: {{post.url}}	
{% endfor %}
