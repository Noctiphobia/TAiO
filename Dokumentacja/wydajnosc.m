X = [200, 400, 600];
najmniej_dziur_10 = [2.6, 9.3, 21.3];
najmniej_dziur_8 = [2.1, 6.5, 15.4];
najmniej_dziur_6 = [1.8, 5.4, 13.0];
najmniej_dziur_4 = [1.4, 4.0, 8.2];
najmniej_dziur_3 = [1.1, 3.1, 7.2];
najmniej_dziur_2 = [1.1, 3.1, 7.1];
najmniej_dziur_1 = [1.1, 3.1, 7.3];
wys = [1.4, 4.5, 8.9];
przyl = [4.5, 16.1, 33.8];


% plot(X, najmniej_dziur_4, '*-g')
% hold on
% grid minor
% xlabel('Liczba klocków')
% ylabel('Czas obliczeñ')
% xlim([0, 800])
% ylim([0, 35])
% plot(X, wys, '*-b')
% plot(X, przyl, '*-r')
% legend('show')
% legend('Najmniej dziur', 'Najmniejsza wysokoœæ', 'Najwiêksza przyleg³oœæ')
% hold off


plot(X, najmniej_dziur_1, '*-g')
hold on
plot(X, najmniej_dziur_2, '*-c')
plot(X, najmniej_dziur_3, '*-b')
plot(X, najmniej_dziur_4, '*-m')
plot(X, najmniej_dziur_6, '*-r')
plot(X, najmniej_dziur_8, '*-y')
plot(X, najmniej_dziur_10, '*-k')
grid minor
xlabel('Liczba klocków')
ylabel('Czas obliczeñ')
xlim([0, 800])
ylim([0, 25])
legend('show')
legend('k = 1', 'k = 2', 'k = 3', 'k = 4', 'k = 6', 'k = 8', 'k = 10')
hold off