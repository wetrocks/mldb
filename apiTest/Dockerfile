FROM postman/newman AS newman

WORKDIR /etc/newman
COPY ./*.json ./

COPY --chmod=555 ./wait-for ./

ENTRYPOINT ["sh", "-c"]
