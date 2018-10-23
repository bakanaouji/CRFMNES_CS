# CRFMNES_CS

CR-FM-NES C# implementation

## Getting Started

### Installing

Please run the following command.

```bash
$ nuget install CRFMNES
```

## Example

This is a simple example that objective function is sphere function.

```cpp
using System;
using CRFMNES;
using CRFMNES.Utils;

class MainClass
{
    public static void Main(string[] args)
    {
        double Sphere(Vector x) => x & x;
        int dim = 3;
        Vector mean = Vector.Fill(dim, 0.5);
        double sigma = 0.2;
        int lamb = 6;
        Optimizer optimizer = new Optimizer(dim, Sphere, mean, sigma, lamb);
        for (int i = 0; i < 100; ++i)
        {
            optimizer.OneIteration();
        }
        Console.WriteLine("x_best: {0}, f_best: {1}", optimizer.XBest, optimizer.FBest);
    }
}
# x_best: Vector(-4.75987117170772E-12, -1.34712992796176E-11, 2.46825317903529E-11), f_best: 8.13359653434064E-22
```


## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/bakanaouji/CRFMNES_CS/tags). 


## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/bakanaouji/CRFMNES_CS/blob/master/LICENSE) file for details