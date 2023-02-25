#include <functional>
#include <string>
#include <iostream>
#include <vector>
#include <algorithm>
#include <random>
#include <algorithm>
#include <array>
#include <functional>
#include <iomanip>
#include <iostream>

#define DOCTEST_CONFIG_IMPLEMENT_WITH_MAIN
#include "doctest.h"

using namespace std;

auto insertionSort = [](const vector<int> arr)
{
	int i, key, j;

	vector<int> sortResult;

	for (int i = 0; i < (int)arr.size(); i++)
		sortResult.push_back(arr[i]);

	for (i = 1; i < (int)arr.size(); i++)
	{
		key = sortResult[i];
		j = i - 1;

		// Move elements of arr[0..i-1], 
		// that are greater than key, to one
		// position ahead of their
		// current position
		while (j >= 0 && sortResult[j] > key)
		{
			sortResult[j + 1] = sortResult[j];
			j = j - 1;
		}
		sortResult[j + 1] = key;
	}

	return sortResult;
};

auto bucketSort = [](const vector<int> arr, const int bucketsAmount)
{
	vector<vector<int>> buckets(bucketsAmount);
	for (int i = 0; i < (int)arr.size(); ++i)
	{
		buckets[floor(arr[i] / (bucketsAmount + 1))].push_back(arr[i]);
	}

	for (int i = 0; i < bucketsAmount; i++)
	{
		buckets[i] = insertionSort(buckets[i]);
	}

	vector<int> sortedArr;

	for (int i = 0; i < bucketsAmount; ++i)
	{
		sortedArr.insert(sortedArr.end(), buckets[i].begin(), buckets[i].end());
	}
	return sortedArr;
};

TEST_CASE("Array1")
{
	vector<int> arr{0, 3, 4, 11, 4, 6, 5, 8, 7, 9, 10};

	auto res = bucketSort(arr, 4);

	CHECK_EQ(0, res.at(0));
	CHECK_EQ(3, res.at(1));
	CHECK_EQ(4, res.at(2));
	CHECK_EQ(4, res.at(3));
	CHECK_EQ(5, res.at(4));
	CHECK_EQ(6, res.at(5));
	CHECK_EQ(7, res.at(6));
	CHECK_EQ(8, res.at(7));
	CHECK_EQ(9, res.at(8));
	CHECK_EQ(10, res.at(9));
	CHECK_EQ(11, res.at(10));
}

TEST_CASE("Array2")
{
	vector<int> arr{1, 4, 11, 4, 6, 8, 3, 14, 10, 15};

	auto res = bucketSort(arr, 4);

	CHECK_EQ(1, res.at(0));
	CHECK_EQ(3, res.at(1));
	CHECK_EQ(4, res.at(2));
	CHECK_EQ(4, res.at(3));
	CHECK_EQ(6, res.at(4));
	CHECK_EQ(8, res.at(5));
	CHECK_EQ(10, res.at(6));
	CHECK_EQ(11, res.at(7));
	CHECK_EQ(14, res.at(8));
	CHECK_EQ(15, res.at(9));
}