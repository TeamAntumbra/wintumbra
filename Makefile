.PHONY: pull, push, update, docs
pull:
	git pull origin master
push:
	git push origin master
update: pull push
docs:
	mkdocs serve
