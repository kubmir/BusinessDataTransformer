import os
import pandas as pd

from linearmodels import PanelOLS
from linearmodels import RandomEffects
import statsmodels.api as sm

import numpy.linalg as la
from scipy import stats
import numpy as np

from boxplotFilter import filterOutLayers

def baseTestOwnerEffect():
    col_list = [
        "ICO",
        "Rok",
        "ROE",
        "Zahranicny_vlastnik",
        "Institucionalny_vlastnik",
        "Koncentracia_vlastnictva",
        "Jednoosobova_SRO"
    ]

    df = pd.read_csv(os.path.expanduser("~/Desktop/Diplomovka_ESF/final_data/only_complete_roe_3y.csv"), usecols=col_list, delimiter=';', encoding='utf8', index_col=["ICO", "Rok"])

    dfWithoutOutLayer = filterOutLayers(df, "ROE")

    exog_vars = ["Zahranicny_vlastnik", "Institucionalny_vlastnik", "Koncentracia_vlastnictva", "Jednoosobova_SRO"]
    exog = sm.add_constant(dfWithoutOutLayer[exog_vars])
    endog = dfWithoutOutLayer["ROE"]

    # random effects model
    model_re = RandomEffects(endog, exog) 
    re_res = model_re.fit() 

    # fixed effects model
    model_fe = PanelOLS(endog, exog, entity_effects = True) 
    fe_res = model_fe.fit() 

    #print results
    print(re_res)
    print(fe_res)

    # Hausman test
    hausman_results = hausman(fe_res, re_res) 
    
    print("chi-Squared: {}".format(str(hausman_results[0])))
    print("degrees of freedom: {}".format(str(hausman_results[1])))
    print("p-Value: {}".format(str(hausman_results[2])))
    
    return df

def hausman(fe, re):
    b = fe.params
    B = re.params
    v_b = fe.cov
    v_B = re.cov

    df = b[np.abs(b) < 1e8].size
    chi2 = np.dot((b - B).T, la.inv(v_b - v_B).dot(b - B))
    pval = stats.chi2.sf(chi2, df)
    
    return chi2, df, pval

def main():
    baseTestOwnerEffect()

if __name__ == "__main__":
    main()