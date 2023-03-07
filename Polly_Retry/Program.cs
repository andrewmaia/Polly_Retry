using Polly;


int retries = 1;
var policy = Policy.Handle<Exception>().WaitAndRetryAsync(
    3, // 3 tentativas
    attempt => TimeSpan.FromMilliseconds(2000), // Espera 2 segundos entre cada tentativa
    (exception, calculatedWaitDuration) => 
    {
        Console.WriteLine($"Erro tentativa {retries}: " + exception.Message);
        retries++;
    }
);


using (var client = new HttpClient())
{
    // O trecho abaixo será executado até 3x caso ocorram erros. A politica só irá "estourar" o erro depois da terceira tentativa
    await policy.ExecuteAsync(async () =>
    {
            var msg = await client.GetStringAsync("www.g2.com.br/api/values/");
    });
}
