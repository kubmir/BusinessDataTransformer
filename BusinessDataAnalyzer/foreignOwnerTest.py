from dataLoader import loadDataForOwnerTest
from scipy.stats import levene, describe, ttest_ind
import pandas as pd
import os

analyzedYear = "2014"

def loadDataFramesFromFile():
    df = loadDataForOwnerTest(analyzedYear)

    return df

def isValidOwnershipSize(value):
    try:
        numberValue = float(value)

        return numberValue > 20
    except ValueError:
        return False

def splitData(df, year):
    foreign = []
    domestic = []

    foreignDF = []
    domesticDF = []

    for index, row in df.iterrows():
        currentRoa = float(row["ROA{}".format(year)])

        if pd.isna(currentRoa) or currentRoa > 3 or currentRoa < -3:
            continue

        if row["{}-1.Majitel-KrajinaPriznak".format(year)] == 'ZAHR':
            if isValidOwnershipSize(row["{}-1.Majitel-Podiel".format(year)]):
                foreign.append(currentRoa)
                foreignDF.append(row)
            continue

        if row["{}-2.Majitel-KrajinaPriznak".format(year)] == 'ZAHR':
            if isValidOwnershipSize(row["{}-2.Majitel-Podiel".format(year)]):
                foreign.append(currentRoa)
                foreignDF.append(row)
            continue

        if row["{}-3.Majitel-KrajinaPriznak".format(year)] == 'ZAHR':
            if isValidOwnershipSize(row["{}-3.Majitel-Podiel".format(year)]):
                foreign.append(currentRoa)
                foreignDF.append(row)
            continue

        domestic.append(currentRoa)
        domesticDF.append(row)

    print(len(domestic))
    print("vs.")
    print(len(foreign))

    dfForeign = pd.DataFrame(foreignDF)
    dfDomestic = pd.DataFrame(domesticDF)

    dfForeign.to_csv(os.path.expanduser("~/Desktop/Diplomovka_ESF/outputs/foreignOwners.csv"), sep=';')
    dfDomestic.to_csv(os.path.expanduser("~/Desktop/Diplomovka_ESF/outputs/domesticOwners.csv"), sep=';')

    return domestic, foreign


def main():
    alpha = 0.05
    
    df = loadDataFramesFromFile()
    domestic, foreign = splitData(df, analyzedYear)

    print("Domestic stats")
    print(describe(domestic))
    print("Foreign stats")
    print(describe(foreign))

    print("Levene result")
    stat, p = levene(domestic, foreign)

    print(stat)
    print("P value is {}".format(p))

    if p < alpha:  # null hypothesis: the groups we're comparing all have equal population variances
        print("Equal variances can be rejected. We have evidence of significant difference.")
    else:
        print("Equal variances cannot be rejected. We do not have evidence of significant difference.")

    print("Student t-test result")
    t_stat, p_ttest = ttest_ind(domestic, foreign)

    print(t_stat)
    print("P value is {}".format(p_ttest))
    
    if p_ttest < alpha:  # null hypothesis: the groups we're comparing all have equal population variances
        print("Equal variances can be rejected. We have evidence of significant difference.")
    else:
        print("Equal variances cannot be rejected. We do not have evidence of significant difference.")

if __name__ == "__main__":
    main()
