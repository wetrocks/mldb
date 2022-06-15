FROM postman/newman AS newman

WORKDIR /etc/newman
COPY ./*.json .

RUN wget https://raw.githubusercontent.com/eficode/wait-for/v2.2.3/wait-for && chmod +x wait-for

ENTRYPOINT ["sh"]
