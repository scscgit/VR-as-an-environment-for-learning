using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class NumberOrdering : MonoBehaviour
{
    public static readonly int TotalNumbers = 10;
    public static readonly float SizeXOfCrate = 1.55f;

    [SerializeField] private bool _gameStarted;
    [SerializeField] private readonly GameObject[] _crates = new GameObject[TotalNumbers];
    private int _correct;

    public GameObject NumberedCratePrefab;
    public GameObject NumberOrderingCratesTransform;
    public GameObject ExampleCrates;

    public static IList<int> RandomlyOrderedNumbers(int max)
    {
        var random = new Random();
        var orderedNumbers = new List<int>(max);
        var randomNumbers = new List<int>(max);

        for (var i = 0; i < max; i++)
        {
            orderedNumbers.Add(i);
        }

        while (orderedNumbers.Count > 0)
        {
            var index = random.Next(0, orderedNumbers.Count);
            randomNumbers.Add(orderedNumbers[index]);
            orderedNumbers.RemoveAt(index);
        }
        return randomNumbers;
    }

    public void StartGame()
    {
        if (_gameStarted)
        {
            return;
        }
        _gameStarted = true;

        foreach (Transform deletingCrate in ExampleCrates.transform)
        {
            Destroy(deletingCrate.gameObject);
        }

        var randomNumbers = RandomlyOrderedNumbers(TotalNumbers);
        var nextPosition = new Vector3(0f, 0f, 0f);
        foreach (var i in randomNumbers)
        {
            var crate = (GameObject) Instantiate(NumberedCratePrefab, NumberOrderingCratesTransform.transform);
            crate.GetComponent<NumberedCrate>().Number = i;
            crate.transform.localPosition = nextPosition;
            nextPosition = new Vector3(
                nextPosition.x + SizeXOfCrate,
                nextPosition.y,
                nextPosition.z);
            _crates[i] = crate;
        }
    }

    public void EndGame()
    {
        if (!_gameStarted)
        {
            return;
        }
        _gameStarted = false;

        foreach (var crate in _crates)
        {
            crate.GetComponent<NumberedCrate>().DestroyMyselfSafely();
        }
    }

    public bool VerifySuccess()
    {
        if (!_gameStarted)
        {
            return false;
        }
        var verification = new Dictionary<int, float>(TotalNumbers);
        foreach (var crate in _crates)
        {
            var position = crate.transform.localPosition.x;
            var number = crate.GetComponent<NumberedCrate>().Number;
            verification.Add(number, position);
        }

        for (var i = 0; i < verification.Count - 1; i++)
        {
            if (verification[i] > verification[i + 1])
            {
                _correct = i;
                return false;
            }
        }
        return true;
    }

    public string VerificationStatus()
    {
        return _gameStarted
            ? "There " + (_correct == 1 ? "is" : "are") + " " + _correct + " correctly ordered crates."
            : "The number ordering game is not running.";
    }

    private void Update()
    {
        if (VerifySuccess())
        {
            EndGame();
        }

        //Debug.Log(VerificationStatus());
    }
}
