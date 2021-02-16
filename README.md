# cndev

### build and deploy
```bash
kustomize build . | kubectl apply -f -
```

### delete / uninstall
```bash
kustomize build . | kubectl delete -f -
```

### Edit Kustomize file per env
Example to update image tag
```bash
cd kustomization/dev/

kustomize edit set image surenmcode/strings-api=surenmcode/strings-api:5.5

# verify
kustomize build .
```

