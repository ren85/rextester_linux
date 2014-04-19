#include <iostream>
#include <string>
#include <vector>

template<int N>
inline int foo(int x)
{
    if (x == 0)
        return N;
    if (x & 1)
        return foo<(N << 1) + 1>(x >> 1);
    else
        return foo<N << 1>(x >> 1);
}

int main()
{
    std::cout << foo<1>(10);
}