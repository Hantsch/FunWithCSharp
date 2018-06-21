This project simulates the issues having a non concurrent cash like a memory cach in an cloud architecture.
The problem occures when you scale your applications out to more than one instance, so each application has its own memory of course.

If this happens you have to take care by using something like Redis.

Conclusion:
Redis is the best solution here and its open source,
however in an enterprise environment you might use this as a cloud service
and then it can get expensive quite fast.

I  also done a solution with Azure Storage account utilizing a blob storage as a concurrent
cache, this might be a cheaper aproach but keep in mind it might not be the best performing.

Based on that i've made my decision on what to use for our services.