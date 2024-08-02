# 添加基础镜像
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

#容器中系统的工作空间
WORKDIR /app

#设置Docker容器对外暴露的端口
EXPOSE 80

#拷贝当前文件夹下的文件到容器中系统的工作空间
COPY . /app
ENV TZ=Asia/Shanghai
#容器中使用 ["dotnet","系统启动的dll"] 来运行应用程序
ENTRYPOINT ["dotnet", "EIP.Api.dll"]
