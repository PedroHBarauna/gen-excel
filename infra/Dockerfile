FROM mcr.microsoft.com/mssql/server:2022-latest

ENV ACCEPT_EULA=Y
ENV MSSQL_SA_PASSWORD=StrongPassword@123
ENV MSSQL_PID=Developer

WORKDIR /usr/src/app

COPY init.sql /docker-entrypoint-initdb.d/init.sql

CMD /opt/mssql/bin/sqlservr
