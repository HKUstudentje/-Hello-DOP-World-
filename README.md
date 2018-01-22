# -Hello-DOP-World-
Learning all the Data Oriented Programming ways.


### Scoping

Scope your variables as tight as possible.
If variables go out of scope they are freed in memory.

A scope is denoted by curly braces:

```csharp
// GLOBAL SCOPE HERE
{
    // SCOPE 1
    int a = 1;
    
    {
        // SCOPE 1a
        // you have access to variable 'a'
        int b = 2;
        // you have access to 'a' and 'b' now.
    }

    // b is gone now. Only 'a' is known here.

    {
        // SCOPE 1b
        // you have access to variable 'a'
        int c = 3;
        // you have access to 'a' and 'c'
    }
    // c is gone now. Only 'a' is known here.
} // end of SCOPE 1
// GLOBAL SCOPE HERE
```

This is a good example: you grab the attached `Rigidbody` component in a class variable at `Start`.
From then on you can refer to it in each function without having to grab it each time.

```csharp
Rigidbody rb;

void Start() {
    rb = GetComponent<Rigidbody>();
}

void Update() {
    // You can now do something with rb
}
```

Don't grab GetComponent<Rigidbody>() (calling into Unity engine) each tick!
You want to have the same object each time, so it should be scoped outside.

```csharp
void Update() {
    // ugh, don't grab it each tick :(
    Rigidbody rb = GetComponent<Rigidbody>();   
}
```

### On using code comments
An important thing to remember: "code-comment is code that is not checked by the compiler".
Your comments will be the first thing to be out-dated, after which it will only makes stuff harder to understand.
A lot of comments make your code hard to read.

In general, only comment code on a meta level (e.g. about what a class does, or why a function does is there).
If you code well, your code itself is the best comment.
Your code should be self-explanatory through clean coding and good variable names.
